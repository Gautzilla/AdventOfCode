from template_creator import read_input
from pandas import Timestamp, Timedelta
import re

PATTERN = r"mul\((?P<n1>\d{1,3}),(?P<n2>\d{1,3})\)"
PATTERNS_PART2 = rf"(?P<operation>{PATTERN})|(?P<disable>don't)|(?P<enable>do(?!n't))"

def part1(memory: str) -> any:
	values = re.findall(PATTERN, memory)
	return sum(int(a) * int(b) for a, b in values)

def part2(memory: str) -> any:
	is_enabled = True
	sum = 0
	for match in re.finditer(PATTERNS_PART2, memory):
		groups = match.groupdict()
		if groups["enable"]:
			is_enabled = True
		if groups["disable"]:
			is_enabled = False
		if is_enabled and (mul := groups["operation"]):
			sum += part1(mul)
	return sum

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