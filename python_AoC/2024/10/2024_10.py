from debugpy.common.timestamp import current

from template_creator import read_input
from pandas import Timestamp, Timedelta

def neighbors(grid,x,y,current_height):
	directions = ((0,1), (1,0), (0,-1), (-1,0))
	possible_neighbors = ((x+d[0], y+d[1]) for d in directions)
	return [n for n in possible_neighbors if is_in_grid(grid,*n) and grid[n[1]][n[0]] == current_height+1]

def is_in_grid(grid, x, y) -> bool:
	return 0 <= y < len(grid) and 0 <= x < len(grid[0])

def hike(grid, x, y, current_height, tops: set):
	if current_height == 9:
		tops.add((x, y))
		return 1
	reachable_neighbors = neighbors(grid,x,y,current_height)
	return sum(hike(grid, xn, yn, current_height+1, tops) for xn,yn in reachable_neighbors)

def solve_grid(grid: list[list[int]]) -> tuple[int,int]:
	part_one = 0
	part_two = 0
	for y in range(len(grid)):
		for x in range(len(grid[0])):
			if grid[y][x] == 0:
				tops = set()
				part_two += hike(grid, x,y, 0,tops)
				part_one += len(tops)
	return part_one, part_two

def solve(puzzle_input: str) -> tuple[any, any]:
	grid = [[int(c) for c in line] for line in puzzle_input.splitlines()]
	return solve_grid(grid)

if __name__ == "__main__":
	i = read_input(example = False)

	t_start = Timestamp.now()
	p1, p2 = solve(puzzle_input=i)
	t_stop = Timestamp.now()
	t = Timedelta(t_stop - t_start).total_seconds()

	print(f"{'Part one':<20}{p1:>10}")
	print(f"{'Part two':<20}{p2:>10}")

	print(f"\nSolved in {t} second{'s' if t >= 2 else ''}.")