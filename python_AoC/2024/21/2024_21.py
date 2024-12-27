from template_creator import read_input
from pandas import Timestamp, Timedelta

MAIN_KEYBOARD = {
	"0": ["","^<","^","^>","^^<","^^","^^>","^^^<","^^^","^^^>",">"],
	"1": [">v","",">",">>","^","^>","^>>","^^","^^>","^^>>",">>v"],
	"2": ["v","<","",">","<^","^","^>","<^^","^^","^^>","v>"],
	"3": ["<v","<<","<","","<<^","<^","^","<<^^","<^^","^^","v"],
	"4": [">vv","v","v>","v>>","",">",">>","^","^>","^>>",">>vv"],
	"5": ["vv","<v","v","v>","<","",">","<^","^","^>","vv>"],
	"6": ["<vv","<<v","<v","v","<<","<","","<<^","<^","^","vv"],
	"7": [">vvv","vv","vv>","vv>>","v","v>","v>>","",">",">>",">>vvv"],
	"8": ["vvv","<vv","vv","vv>","<v","v","v>","<","",">","vvv>"],
	"9": ["<vvv","<<vv","<vv","vv","<<v","<v","v","<<","<","","vvv"],
	"A": ["<","^<<","<^","^","^^<<","<^^","^^","^^^<<","<^^^","^^^",""]
}

DIR_KEYBOARD = {
	"^": {
		"^": "",
		">": "v>",
		"v": "v",
		"<": "v<",
		"A": ">"
	},
	">": {
		"^": "<^",
		">": "",
		"v": "<",
		"<": "<<",
		"A": "^"
	},
	"v": {
		"^": "^",
		">": ">",
		"v": "",
		"<": "<",
		"A": "^>"
	},
	"<": {
		"^": ">^",
		">": ">>",
		"v": ">",
		"<": "",
		"A": ">>^"
	},
	"A": {
		"^": "<",
		">": "v",
		"v": "<v",
		"<": "v<<",
		"A": ""
	},
}



def drive_robots(puzzle_input: str, nb_robots: int) -> int:
	output = {}
	for input in puzzle_input.splitlines():
		sequence = ""
		sequence2 = ""
		cc = "A"
		for c in input:
			i = int(c) if c.isdigit() else 10
			sequence += MAIN_KEYBOARD[cc][i] + "A"
			cc = c
		for _ in range(nb_robots):
			cc = "A"
			for c in sequence:
				sequence2 += DIR_KEYBOARD[cc][c] + "A"
				cc = c
			sequence = sequence2
			sequence2 = ""
		output[input] = sequence
	return sum(int(key[:-1]) * len(value) for key,value in output.items())

def part1(puzzle_input: str) -> any:
	return drive_robots(puzzle_input, 2)

def part2(puzzle_input: str) -> any:
	return drive_robots(puzzle_input, 25)

def solve(puzzle_input: str) -> tuple[any, any]:
	part_one = part1(puzzle_input)
	part_two = "" # part2(puzzle_input)

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