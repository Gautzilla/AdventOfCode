from template_creator import read_input
from pandas import Timestamp, Timedelta
from functools import reduce

def surface(faces: list[int]) -> int:
	side_ares = [2*faces[i]*faces[i-1] for i in range(3)]
	return sum(side_ares) + min(side_ares) // 2

def ribbon_length(faces: list[int]) -> int:
	wrap = sum(2*f for f in sorted(faces)[:-1])
	bow = reduce(lambda x,y: x*y, faces)
	return wrap + bow

def part1(puzzle_input) -> int:
	return sum(surface(box) for box in puzzle_input)

def part2(puzzle_input) -> int:
	return sum(ribbon_length(box) for box in puzzle_input)

def solve(puzzle_input: str) -> tuple[any, any]:
	puzzle_input = list([int(dimension) for dimension in line.split("x")] for line in puzzle_input.split())
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
	print(f"\nSolved in {t} second.")