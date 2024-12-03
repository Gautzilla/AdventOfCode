from template_creator import read_input
from pandas import Timestamp, Timedelta
import re

PATTERN = r"mul\((?P<n1>\d{1,3}),(?P<n2>\d{1,3})\)"

def part1(memory: str) -> any:
	values = re.findall(PATTERN, memory)
	return sum(int(a) * int(b) for a, b in values)

def remove_disabled_parts(memory: str) -> str:
	if "don't" not in memory:
		return memory
	dont_index = memory.index("don't")
	do_index = list(i.regs[0][1] for i in re.finditer("do(?!n't)", memory))
	do_index = [idx for idx in do_index if idx > dont_index]
	do_index = len(memory) - 1 if not do_index else do_index[0]
	return remove_disabled_parts(memory[:dont_index] + memory[do_index:])

def part2(memory: str) -> any:
	memory = remove_disabled_parts(memory)
	return part1(memory)

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