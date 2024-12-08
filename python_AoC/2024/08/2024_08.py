from template_creator import read_input
from pandas import Timestamp, Timedelta
from itertools import combinations

def antinodes_coordinates(antenna1: tuple[int, int], antenna2: tuple[int,int]) -> list[tuple[int,int]]:
	return [(2*a1[0]-a2[0], 2*a1[1]-a2[1]) for (a1,a2) in [(antenna1,antenna2), (antenna2,antenna1)]]

def resonant_antinodes_coordinates(antenna1: tuple[int, int], antenna2: tuple[int,int], grid_size: tuple[int,int]) -> list[tuple[int,int]]:
	antinodes = [antenna1, antenna2]

	for (a1, a2) in [(antenna1, antenna2), (antenna2, antenna1)]:
		xdiff = a1[0]-a2[0]
		ydiff = a1[1]-a2[1]
		antinode = a1

		while True:
			antinode = (antinode[0] + xdiff, antinode[1] + ydiff)
			if not is_in_grid(antinode, grid_size):
				break
			antinodes.append(antinode)

	return antinodes

def is_in_grid(antinode: tuple[int,int], grid_size: tuple[int,int]) -> bool:
	return 0<=antinode[0]<grid_size[0] and 0<=antinode[1]<grid_size[1]

def part1(antennas: dict[str, list[tuple[int,int]]], grid_size) -> any:
	antinodes = set()
	for antenna_coordinates in antennas.values():
		for a1,a2 in combinations(antenna_coordinates, 2):
			antinodes |= set(antinode for antinode in antinodes_coordinates(a1,a2) if is_in_grid(antinode, grid_size))
	return len(antinodes)

def part2(antennas: dict[str, list[tuple[int,int]]], grid_size) -> any:
	antinodes = set()
	for antenna_coordinates in antennas.values():
		for a1, a2 in combinations(antenna_coordinates, 2):
			antinodes |= set(resonant_antinodes_coordinates(a1, a2, grid_size))

	return len(antinodes)

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
	part_two = part2(antennas, grid_size)

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