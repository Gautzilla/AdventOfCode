from template_creator import read_input
from pandas import Timestamp, Timedelta

def mix(secret_numer: int, value: int) -> int:
	return secret_numer ^ value

def prune(secret_number: int) -> int:
	return secret_number % 16777216

def evolve(secret_number: int) -> int:
	secret_number = mix(secret_number, secret_number * 64)
	secret_number = prune(secret_number)
	secret_number = mix(secret_number, secret_number // 32)
	secret_number = prune(secret_number)
	secret_number = mix(secret_number, secret_number * 2048)
	return prune(secret_number)

def part1(secret_numbers: list[int]) -> any:
	for _ in range(2000):
		secret_numbers = [evolve(secret_number) for secret_number in secret_numbers]
	return sum(secret_numbers)

sequences = {}

def part2(secret_numbers: list[int]) -> any:
	for idx, buyer in enumerate(secret_numbers):
		last_prices = [None, None, None, None, None]
		secret_number = buyer
		for _ in range(2000):
			last_prices.pop(0)
			price = secret_number%10
			last_prices.append(price)
			secret_number = evolve(secret_number)
			if None in last_prices:
				continue
			sequence = tuple(last_prices[i] - last_prices[i-1] for i in range(1, len(last_prices)))
			if sequence not in sequences:
				sequences[sequence] = [None] * len(secret_numbers)
			if sequences[sequence][idx] is not None:
				continue
			sequences[sequence][idx] = price
	return max(sum(vi for vi in v if vi is not None) for v in sequences.values())

def solve(puzzle_input: str) -> tuple[any, any]:
	secret_numbers = list(map(int, puzzle_input.splitlines()))
	part_one = part1(secret_numbers)
	part_two = part2(secret_numbers)

	return part_one, part_two

if __name__ == "__main__":
	i = read_input(example = False)

	t_start = Timestamp.now()
	p1, p2 = solve(puzzle_input=i)
	t_stop = Timestamp.now()
	t = Timedelta(t_stop - t_start).total_seconds()

	print(f"{'Part one':<20}{p1:>10}")
	print(f"{'Part two':<20}{p2:>10}")

	print(f"\nSolved in {t} second{'s' if t >= 2 else ''}.")