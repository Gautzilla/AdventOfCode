with open("example_input.txt") as e:
	example_input = e.read()

with open("puzzle_input.txt") as i:
	puzzle_input = i.read()

#i = example_input
i = puzzle_input

lines = [[int(v) for v in line.split(" ") if v] for line in i.split("\n")]
list_one = sorted(line[0] for line in lines)
list_two = sorted(line[1] for line in lines)
comparison = zip(list_one, list_two)
print(sum(abs(c[1] - c[0]) for c in comparison))