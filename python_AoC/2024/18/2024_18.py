from template_creator import read_input
from pandas import Timestamp, Timedelta

directions = {(0,1),(1,0),(0,-1),(-1,0)}


def part1(obstacles: list[list[int]], grid_size: int) -> any:
	starting_position = (0,0)
	destination = (grid_size,grid_size)
	queue = [(starting_position, [])]
	visited = set()
	while len(queue) > 0:
		current_position, path = queue.pop(0)
		visited.add(current_position)
		path.append(current_position)
		if current_position == destination:
			return path
		for direction in directions:
			neighbour = tuple(map(sum,zip(current_position, direction)))
			if neighbour in obstacles:
				continue
			if neighbour in (n for n,_ in queue):
				continue
			if neighbour in visited:
				continue
			if not (0 <= neighbour[0] <= grid_size and 0 <= neighbour[1] <= grid_size):
				continue
			queue.append((neighbour, [*path]))
	return None

def solve(puzzle_input: str, example: int) -> tuple[any, any]:
	nb_obstacles, grid_size = (12, 6) if example else (1024, 70)
	obstacles = list(tuple(map(int, input.split(","))) for input in puzzle_input.splitlines())
	part_one = part1(obstacles[:nb_obstacles], grid_size)

	for i in range(len(obstacles))[::-1]:
		if part1(obstacles[:i], grid_size) is not None:
			break
	last_obtacle = obstacles[i]

	return len(part_one)-1, ",".join(map(str, last_obtacle))

if __name__ == "__main__":
	example = False
	i = read_input(example)

	t_start = Timestamp.now()
	p1, p2 = solve(puzzle_input=i, example=example)
	t_stop = Timestamp.now()
	t = Timedelta(t_stop - t_start).total_seconds()

	print(f"{'Part one':<20}{p1:>10}")
	print(f"{'Part two':<20}{p2:>10}")

	print(f"\nSolved in {t} second{'s' if t >= 2 else ''}.")