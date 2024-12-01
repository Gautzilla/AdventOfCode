from template_creator import read_input
from pandas import Timestamp, Timedelta

directions = {
	">": (1,0),
	"^": (0,1),
	"v": (0,-1),
	"<": (-1,0),
}

def deliver(instructions: str, position: tuple[int, int], visited_houses: set) -> None:
	visited_houses.add(position)
	for c in instructions:
		direction = directions[c]
		position = (position[0] + direction[0], position[1] + direction[1])
		visited_houses.add(position)

def part1(puzzle_input: str) -> int:
	visited_houses = set()
	deliver(instructions=puzzle_input, position=(0,0), visited_houses=visited_houses)
	return len(visited_houses)

def part2(puzzle_input: str) -> int:
	visited_houses = set()
	deliver(instructions=puzzle_input[::2], position=(0, 0), visited_houses=visited_houses)
	deliver(instructions=puzzle_input[1::2], position=(0, 0), visited_houses=visited_houses)
	return len(visited_houses)

def solve(puzzle_input: str) -> tuple[any, any]:
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