from template_creator import read_input
from pandas import Timestamp, Timedelta
from itertools import combinations

def part1(puzzle_input: str) -> any:
	computers = {}
	for line in puzzle_input.splitlines():
		first, second = line.split("-")
		for a,b in (first, second), (second, first):
			if a not in computers:
				computers[a] = [b]
				continue
			computers[a].append(b)
	combs = set()
	for computer, links in computers.items():
		combs |= set(tuple(sorted(c)) for c in (combinations([computer] + links, 3)))

	combs = (c for c in combs if any(s[0] == "t" for s in c))
	combs = (c for c in combs if all(c2 in computers[c1] for c1 in c for c2 in (c3 for c3 in c if c3 != c1)))
	return sum(1 for _ in combs)

def part2(puzzle_input: str) -> any:
	return ""

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

	print(f"\nSolved in {t} second{'s' if t >= 2 else ''}.")