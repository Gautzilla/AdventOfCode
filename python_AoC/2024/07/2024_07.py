from template_creator import read_input
from pandas import Timestamp, Timedelta

def is_solvable(result: int, numbers: list[int], current: int, index: int, part: int):
	if len(numbers) == 0:
		return current == result
	if current > result:
		return False
	if part == 1:
		return is_solvable(result, numbers[1:], current+numbers[0], index+1,part) or is_solvable(result, numbers[1:], current*numbers[0], index+1,part)
	return is_solvable(result, numbers[1:], current + numbers[0], index + 1,part) or is_solvable(result, numbers[1:],
																							current * numbers[0],
																							index + 1,part) or is_solvable(result, numbers[1:],
																							int(str(current)+str(numbers[0])),
																							index + 1,part)

def part1(equations: list[tuple[int, list[int]]]) -> any:
	return sum(equation[0] for equation in equations if is_solvable(equation[0], equation[1][1:], equation[1][0], 1,1))

def part2(equations: list[tuple[int, list[int]]]) -> any:
	return sum(equation[0] for equation in equations if is_solvable(equation[0], equation[1][1:], equation[1][0], 1,2))

def solve(puzzle_input: str) -> tuple[any, any]:
	results = [int(line.split(":")[0]) for line in puzzle_input.splitlines()]
	numbers = [[int(v) for v in line.split(":")[1].split()] for line in puzzle_input.splitlines()]
	equations = list(zip(results, numbers))
	part_one = part1(equations)
	part_two = part2(equations)

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