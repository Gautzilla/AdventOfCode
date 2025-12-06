from template_creator import run_puzzle

def compute(numbers: list[int], operator: str) -> int:
    operations = {
        "+": lambda a,b: a+b,
        "*": lambda a,b: a*b
    }
    operation = operations[operator]
    for number in numbers[1:]:
        numbers[0] = operation(numbers[0], number)
    return numbers[0]

def part1(numbers: list[list[int]], operators: list[str]) -> any:
    return sum(compute(n, operator) for *n, operator in zip(*numbers, operators))


def part2(puzzle_input: str) -> any:
    return ""

def parse_input(puzzle_input: str) -> tuple:
    numbers = list(list(map(int,(number for number in line.split(" ") if number))) for line in puzzle_input.splitlines()[:-1])
    operators = list(line for line in puzzle_input.splitlines()[-1].split(" ") if line)
    return numbers, operators

def solve(puzzle_input: str) -> tuple[any, any]:
    numbers, operators = parse_input(puzzle_input)
    part_one = part1(numbers, operators)
    part_two = part2(puzzle_input)

    return part_one, part_two


if __name__ == "__main__":
    run_puzzle(solve_function=solve, example=False)
