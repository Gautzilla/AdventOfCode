from template_creator import read_input
from pandas import Timestamp, Timedelta
import hashlib

def find_hash_with_leading_zeroes(puzzle_input: str, nb_zeroes: int) -> int:
	output = 0
	while str(hashlib.md5(f"{puzzle_input}{output}".encode()).hexdigest())[:nb_zeroes] != "0" * nb_zeroes:
		output += 1
	return output

def part1(puzzle_input: str) -> any:
	return find_hash_with_leading_zeroes(puzzle_input=puzzle_input, nb_zeroes=5)

def part2(puzzle_input: str) -> any:
	return find_hash_with_leading_zeroes(puzzle_input=puzzle_input, nb_zeroes=6)

def solve(puzzle_input: str) -> tuple[any, any]:
	part_one = part1(puzzle_input)
	part_two = part2(puzzle_input)

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