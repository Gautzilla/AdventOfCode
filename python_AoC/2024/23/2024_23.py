from template_creator import read_input
from pandas import Timestamp, Timedelta
from itertools import combinations

def are_all_linked(computers: set, links: dict) -> bool:
	return all(comp2 in links[comp1] for comp1 in computers for comp2 in computers - {comp1})

def part1(computer_links: dict) -> any:
	combs = set()
	for computer, links in computer_links.items():
		combs |= set(tuple(sorted(c)) for c in (combinations([computer] + links, 3)))
	combs = (c for c in combs if any(s[0] == "t" for s in c))
	combs = (c for c in combs if are_all_linked(set(c), computer_links))
	return sum(1 for _ in combs)

def part2(computer_links: dict) -> any:
	max_comb = []
	for computer, links in computer_links.items():
		comb_length = len(links) + 1
		while comb_length > len(max_comb):
			combs = combinations([computer] + links, comb_length)
			for comb in combs:
				if are_all_linked(set(comb), computer_links):
					max_comb = list(sorted(comb))
			comb_length -= 1

	return ",".join(max_comb)

def solve(puzzle_input: str) -> tuple[any, any]:
	computer_links = {}
	for line in puzzle_input.splitlines():
		first, second = line.split("-")
		for a, b in (first, second), (second, first):
			if a not in computer_links:
				computer_links[a] = [b]
				continue
			computer_links[a].append(b)

	part_one = part1(computer_links)
	part_two = part2(computer_links)

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