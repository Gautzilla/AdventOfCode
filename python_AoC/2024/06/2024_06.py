from template_creator import read_input
from pandas import Timestamp, Timedelta

DIRECTIONS = [(1,0), (0,1), (-1,0), (0,-1)]
DIRECTIONS_STR = ">v<^"

def parse_grid(puzzle_input: str):
	obstacles = set()
	position = (0,0)
	direction = 0
	size = (len(puzzle_input.splitlines()[0]),len(puzzle_input.splitlines()))
	for y, line in enumerate(puzzle_input.splitlines()):
		for x, c in enumerate(line):
			if c == "#":
				obstacles.add((x,y))
			if c in DIRECTIONS_STR:
				position = (x,y)
				direction = DIRECTIONS_STR.index(c)
	return obstacles, position, direction, size

def run_path(obstacles, position, direction, grid_size):
	visited = set()

	while True:
		if (position, direction) in visited:
			return set(coord for coord, direction in visited), 1
		visited.add((position, direction))
		next_pos = (position[0] + DIRECTIONS[direction][0], position[1] + DIRECTIONS[direction][1])
		if not (0 <= next_pos[0] < grid_size[0] and 0 <= next_pos[1] < grid_size[1]):
			return set(coord for coord, direction in visited), 0
		if next_pos in obstacles:
			direction = (direction + 1) % len(DIRECTIONS)
		else:
			position = next_pos

def part1(obstacles, position, direction, grid_size) -> any:
	return run_path(obstacles, position, direction, grid_size)[0]


def part2(obstacles, position, direction, grid_size, guard_path) -> any:
	nb_loops = 0
	guard_path = set(p for p in guard_path if p != direction)
	for coord in guard_path:
		new_obstacles = obstacles.copy()
		new_obstacles.add(coord)
		nb_loops += run_path(new_obstacles, position, direction, grid_size)[1]
	return nb_loops

def solve(puzzle_input: str) -> tuple[any, any]:
	obstacles, position, direction, grid_size = parse_grid(puzzle_input)
	visited = part1(obstacles, position, direction, grid_size)
	part_two = part2(obstacles, position, direction, grid_size, visited)

	return len(visited), part_two

if __name__ == "__main__":
	i = read_input(example = False)

	t_start = Timestamp.now()
	p1, p2 = solve(puzzle_input=i)
	t_stop = Timestamp.now()
	t = Timedelta(t_stop - t_start).total_seconds()

	print(f"{'Part one':<20}{p1:>10}")
	print(f"{'Part two':<20}{p2:>10}")

	print(f"\nSolved in {t} second{'s' if t >= 2 else ''}.")