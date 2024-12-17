from template_creator import read_input
from pandas import Timestamp, Timedelta
import re

def combo(operand: int, register):
	if operand < 4:
		return operand
	if operand == 7:
		raise ValueError("Operand 7 should not appear in valid programs.")
	return register[{4:"A", 5:"B", 6:"C"}[operand]]

def operation(opcode: int, operand: int, register: dict, program_output: list):
	if opcode == 0:
		register["A"] = register["A"] // 2**(combo(operand, register))
		return None
	if opcode == 1:
		register["B"] = register["B"] ^ operand
		return None
	if opcode == 2:
		register["B"] = (combo(operand, register) % 8)
	if opcode == 3:
		if register["A"] == 0:
			return None
		return operand
	if opcode == 4:
		register["B"] = register["B"] ^ register["C"]
		return None
	if opcode == 5:
		program_output.append(combo(operand, register)%8)
		return None
	if opcode == 6:
		register["B"] = register["A"] // 2 ** (combo(operand, register))
		return None
	register["C"] = register["A"] // 2 ** (combo(operand, register))
	return None


def part1(register, program) -> any:
	program_output = []
	idx = 0
	while idx < len(program):
		pointer = operation(program[idx], program[idx+1], register, program_output)
		idx = idx+2 if pointer is None else pointer
	return ",".join(map(str,program_output))

def part2(program, current_a) -> any:
	if len(program) == 0:
		return current_a
	for b in range(8):
		a = current_a * 8 + b
		a_candidate = a
		b = b ^ 7
		c = a // (2**b)
		a = a // (2**3)
		b = b ^ c
		b = b ^ 7
		if b % 8 == program[-1]:
			if (answer := part2(program[:-1], a_candidate)) is not None:
				return answer
			continue

def solve(puzzle_input: str) -> tuple[any, any]:
	puzzle_input = puzzle_input.splitlines()
	register = {g.group("name"): int(g.group("value")) for g in (re.search(r"Register (?P<name>[ABC]): (?P<value>\d+)", reg) for reg in puzzle_input[:3])}
	program = [int(i) for i in puzzle_input[-1].split()[-1].split(",")]
	part_one = part1(register, program)
	part_two = part2(program, 0)

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