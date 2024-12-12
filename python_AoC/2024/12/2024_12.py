from template_creator import read_input
from pandas import Timestamp, Timedelta
def neighbours(x: int, y: int):
	directions = ((0,1), (1,0), (0,-1), (-1,0))
	return [(x+dir[0], y+dir[1], dir) for dir in directions]

def is_in_grid(x: int, y: int, grid):
	return 0 <= y < len(grid) and 0 <= x < len(grid[0])

def find_region(plant: str, next_nodes : set[(int,int)], grid):
	perimeter = set()
	region = set()
	while next_nodes:
		node = next_nodes.pop()
		region.add(node)
		for x,y, direction in neighbours(*node):
			if (x,y) in region:
				continue
			if not is_in_grid(x,y, grid):
				perimeter.add((x,y,direction))
				continue
			if grid[y][x] != plant:
				perimeter.add((x,y,direction))
				continue
			next_nodes.add((x,y))
	return region, perimeter


def find_all_regions(grid: list[list[str]]) -> any:
	regions : list[set[int], set[int,int,tuple[int,int]]] = []
	visited = set()
	for y in range(len(grid)):
		for x in range(len(grid[0])):
			if (x,y) in visited:
				continue
			plant = grid[y][x]
			regions.append(find_region(plant,{(x,y)}, grid))
			visited |= regions[-1][0]

	return regions

def is_neighbour_fence(f1, f2):
	neighbours = {
		"horizontal": lambda x1,y1,x2,y2: y1 == y2 and abs(x1 - x2) == 1,
		"vertical": lambda x1,y1,x2,y2: x1 == x2 and abs(y1 - y2) == 1
	}
	x1,y1,d1 = f1
	x2,y2,d2 = f2
	d = "horizontal" if abs(d1[1]) == 1 else "vertical"
	return d1 == d2 and neighbours[d](x1,y1,x2,y2)

def find_fence(x,y,direction, fences):
	fence = set(f for f in fences if f[-1] == direction)
	to_visit = {(x, y, direction)}
	same_fence = set()

	while to_visit:
		fence_part = to_visit.pop()
		fence.remove(fence_part)
		same_fence.add(fence_part)
		for neighbour_fence in (f for f in fence if is_neighbour_fence(fence_part, f)):
			to_visit.add(neighbour_fence)
	return same_fence

def nb_sides(fences: set[int,int,tuple[int,int]]) -> int:
	fences = fences.copy()
	nb_sides = 0
	while(fences):
		fence = find_fence(*list(fences)[0], fences)
		fences -= fence
		nb_sides += 1
	return nb_sides

def solve(puzzle_input: str) -> tuple[any, any]:
	grid = [list(line) for line in puzzle_input.splitlines()]
	part_one = sum(len(r[0]) * len(r[1]) for r in find_all_regions(grid))
	part_two = sum(len(region) * nb_sides(fences) for region,fences in find_all_regions(grid))

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