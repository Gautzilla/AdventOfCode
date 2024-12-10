from template_creator import read_input
from pandas import Timestamp, Timedelta

def part1(puzzle_input: str) -> any:
	file = []
	empty_blocks = []
	empty_block_idx = 0
	for i,c in enumerate(puzzle_input):
		if i % 2:
			empty_blocks += list(range(empty_block_idx, empty_block_idx + int(c)))
		else:
			file += [i//2]*int(c)
		empty_block_idx += int(c)
	for i in empty_blocks:
		if i >= len(file):
			break
		file.insert(i, file.pop())

	return sum(int(c)*i for i,c in enumerate(file))

def part2(puzzle_input: str) -> any:
	files = []
	empty = []
	idx = 0
	for i,c in enumerate(puzzle_input):
		if i % 2:
			empty.append([*range(idx, idx + int(c))])
		else:
			files.append([*range(idx, idx + int(c))])
		idx += int(c)

	for index, file in list(enumerate(files.copy()))[::-1]:
		for empty_index in range(len(empty)):
			if len(empty[empty_index]) >= len(file) and empty[empty_index][0] < file[0]:
				files[index] = empty[empty_index][:len(file)]
				empty[empty_index] = empty[empty_index][len(file):]
				break

	return sum(int(f) * i for i,file in enumerate(files) for f in file)

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