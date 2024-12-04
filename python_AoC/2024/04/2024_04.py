from template_creator import read_input
from pandas import Timestamp, Timedelta
import numpy as np

all_directions = [(0, 1), (1, 1), (1, 0), (-1, 1), (-1, 0), (-1, -1), (0, -1), (1, -1)]
diagonals = [[(-1,-1), (0,0), (1,1)], [(-1,1), (0,0), (1,-1)]]

def parse_input(input: str) -> np.ndarray:
	return np.array([[letter for letter in line] for line in input.splitlines()])

def letter_positions(grid: np.ndarray, letter: str) -> list[tuple[int,int]]:
	return list((int(x), int(y)) for x,y in zip(*np.where(grid == letter)))

def is_in_grid(grid: np.ndarray, coordinates: tuple[int,int]) -> bool:
	return all(0 <= coordinates[axis] < grid.shape[axis] for axis in (0,1))

def count_xmas_spelled(grid: np.ndarray, coordinates: tuple[int,int]) -> int:
	return sum(spells_xmas(grid, coordinates, direction) for direction in all_directions)

def spells_xmas(grid: np.ndarray, coordinates: tuple[int,int], direction: tuple[int,int]) -> bool:
	target = "XMAS"
	for target_char in target:
		if not is_in_grid(grid, coordinates):
			return False
		if grid[coordinates] != target_char:
			return False
		coordinates = (coordinates[0] + direction[0], coordinates[1] + direction[1])
	return True

def is_x_shaped_mas(grid: np.ndarray, coordinates: tuple[int,int]) -> bool:
	target = "MAS"
	for diagonal in diagonals:
		all_coordinates = [(coordinates[0] + d[0], coordinates[1] + d[1]) for d in diagonal]
		if not all(is_in_grid(grid, c) for c in all_coordinates):
			return False
		if "".join(str(grid[c]) for c in all_coordinates) not in [target, target[::-1]]:
			return False
	return True

def part1(grid: np.ndarray) -> any:
	return sum(count_xmas_spelled(grid, x) for x in letter_positions(grid, "X"))

def part2(grid: np.ndarray) -> any:
	return sum(1 for a in letter_positions(grid, "A") if is_x_shaped_mas(grid, a))

def solve(puzzle_input: str) -> tuple[any, any]:
	grid = parse_input(puzzle_input)
	part_one = part1(grid)
	part_two = part2(grid)

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