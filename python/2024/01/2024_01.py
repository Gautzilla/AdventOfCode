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

part_one = sum(abs(c[1] - c[0]) for c in comparison)
part_two = sum(v * sum(1 for i in list_two if i == v) for v  in list_one)

print(part_one)
print(part_two)