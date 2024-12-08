from template_creator import read_input
from pandas import Timestamp, Timedelta
from itertools import combinations

def antinodes_coordinates(antenna1: tuple[int, int], antenna2: tuple[int,int]) -> tuple[tuple[int,int], tuple[int,int]]:
	antinode1 = (2*antenna1[0]-antenna2[0], 2*antenna1[1]-antenna2[1])
	antinode2 = (2*antenna2[0]-antenna1[0], 2*antenna2[1]-antenna1[1])
	return antinode1, antinode2

def is_in_grid(antinode: tuple[int,int], grid_size: tuple[int,int]) -> bool:
	return 0<=antinode[0]<grid_size[0] and 0<=antinode[1]<grid_size[1]


def part1(antennas: dict[str, list[tuple[int,int]]], grid_size) -> any:
	antinodes = set()
	for antenna_coordinates in antennas.values():
		for a1,a2 in combinations(antenna_coordinates, 2):
			antinodes |= set(antinode for antinode in antinodes_coordinates(a1,a2) if is_in_grid(antinode, grid_size))

	for y in range(grid_size[1]):
		line = ""
		for x in range(grid_size[0]):
			if any(v:=a for a in antennas if (x,y) in antennas[a]):
				line += v
				continue
			if (x,y) in antinodes:
				line += "#"
				continue
			line += "."
		print(line)

	return len(antinodes)

def part2(puzzle_input: str) -> any:
	return ""

def parse_input(puzzle_input: str) -> dict[str, list[tuple[int,int]]]:
	antennas = {}

	for y, line in enumerate(puzzle_input.splitlines()):
		for x, c in enumerate(line):
			if c == ".":
				continue
			if c in antennas:
				antennas[c].append((x,y))
				continue
			antennas[c] = [(x,y)]

	return antennas

def solve(puzzle_input: str) -> tuple[any, any]:
	antennas = parse_input(puzzle_input)
	grid_size = (len(puzzle_input.splitlines()[0]), len(puzzle_input.splitlines()))
	part_one = part1(antennas, grid_size)
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