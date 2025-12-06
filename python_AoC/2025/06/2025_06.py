from template_creator import run_puzzle

operations = {
    "+": lambda a, b: a + b,
    "*": lambda a, b: a * b
}

def compute(numbers: list[int], operator: str) -> int:
    operation = operations[operator]
    for number in numbers[1:]:
        numbers[0] = operation(numbers[0], number)
    return numbers[0]

def grand_total(numbers: list[list[int]], operators: list[str]) -> any:
    return sum(compute(n, operator) for *n, operator in zip(*numbers, operators))

def part_1_parse_input(puzzle_input: str) -> tuple:
    numbers = list(list(map(int,(number for number in line.split(" ") if number))) for line in puzzle_input.splitlines()[:-1])
    operators = list(line for line in puzzle_input.splitlines()[-1].split(" ") if line)
    return numbers, operators

def part2(puzzle_input: str) -> int:
    lines = puzzle_input.splitlines()
    output = 0
    numbers = []
    for x in range(max(len(line) for line in lines)-1, -1, -1):
        concat_lines = "".join(line[x] for line in lines[:-1] if x < len(line))
        if not concat_lines.strip():
            continue
        numbers.append(int(concat_lines))
        if x < len(lines[-1]) and (operator := lines[-1][x]) in operations:
            output += compute(numbers, operator)
            numbers = []
    return output

def solve(puzzle_input: str) -> tuple[any, any]:
    part_1_numbers, part_1_operators = part_1_parse_input(puzzle_input)
    part_one = grand_total(part_1_numbers, part_1_operators)
    part_two = part2(puzzle_input)

    return part_one, part_two


if __name__ == "__main__":
    run_puzzle(solve_function=solve, example=False)
