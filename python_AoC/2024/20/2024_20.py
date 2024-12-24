from template_creator import read_input
from pandas import Timestamp, Timedelta

DIRECTIONS = {(0,1),(1,0),(0,-1),(-1,0)}
CHEAT_DIRECTIONS = {(1,-1), (2,0), (1,1), (0,2)}

def get_nb_cheats(grid, start, cheat_duration) -> any:
	distances = [[-1] * len(grid[0]) for _ in range(len(grid))]
	distances[start[1]][start[0]] = 0

	x,y = start
	while grid[y][x] != "E":
		for x2,y2 in (map(sum,zip(d,(x,y))) for d in DIRECTIONS):
			if grid[y2][x2] == "#":
				continue
			if distances[y2][x2] != -1:
				continue
			distances[y2][x2] = distances[y][x] + 1
			x,y = x2,y2
			break

	nb_cheats = 0
	seen = set()
	for y, line in enumerate(distances):
		for x, dist in enumerate(line):
			if dist == -1:
				continue
			for dy in range(-1 * cheat_duration, cheat_duration + 1):
				for dx in range(cheat_duration - abs(dy) + 1):
					x2,y2 = x+dx,y+dy
					if not (0 <= x2 < len(grid[0]) and 0 <= y2 < len(grid)):
						continue
					if distances[y2][x2] == -1:
						continue
					seen.add(((x,y),(x2,y2)))
					if ((x2,y2),(x,y)) in seen:
						continue
					if abs(distances[y2][x2] - dist) - dx - abs(dy) >= 100:
						nb_cheats += 1

	return nb_cheats

def part2(puzzle_input: str) -> any:
	return ""

def solve(puzzle_input: str) -> tuple[any, any]:
	grid = [list(line) for line in puzzle_input.splitlines()]
	start = (0,0)
	for y,line in enumerate(grid):
		for x,char in enumerate(line):
			if char == "S":
				start = (x,y)

	part_one = get_nb_cheats(grid, start, 2)
	part_two = get_nb_cheats(grid, start, 20)

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