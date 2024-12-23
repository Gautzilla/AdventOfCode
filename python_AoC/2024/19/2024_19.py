from scipy.cluster.hierarchy import complete

from template_creator import read_input
from pandas import Timestamp, Timedelta

cache = {}

def search(towels, design):
	if design == "":
		return 1
	if design in cache:
		return cache[design]
	for towel in towels:
		if design.startswith(towel) and search(towels, design[len(towel):]):
			adds = search(towels, design[len(towel):])
			if design in cache:
				cache[design] += adds
			else :
				cache[design] = adds
	if design not in cache:
		cache[design] = 0
	return 0 if design not in cache else cache[design]

def part1(towels, designs) -> any:
	return len([design for design in designs if search(list(towels), design)])


def part2(towels, designs) -> any:
	return sum(search(list(towels), design) for design in designs)

def solve(puzzle_input: str) -> tuple[any, any]:
	inp = puzzle_input.splitlines()
	towels = [t.strip() for t in inp[0].split(",")]
	designs = inp[2:]
	part_one = part1(towels, designs)
	part_two = part2(towels, designs)

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