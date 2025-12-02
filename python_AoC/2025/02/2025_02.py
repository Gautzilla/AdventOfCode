from template_creator import run_puzzle

def invalid_ids_p1(ids_str: str) -> list[int]:
    lower, upper = map(int,ids_str.split("-"))
    chunk = int(str(lower)[:max(1,len(str(lower))//2)])
    output = []
    while (id := int(str(chunk)*2)) <= upper:
        if id in range(lower,upper+1):
            output.append(id)
        chunk+=1
    return output

def invalid_ids_p2(ids_str: str) -> list[int]:
    return []

def sum_valid_ids(puzzle_input: str, part: int) -> int:
    invalid_ids = invalid_ids_p1 if part == 1 else invalid_ids_p2
    return sum(
        id for id_str in puzzle_input.split(",") for id in invalid_ids(id_str)
    )

def part1(puzzle_input: str) -> int:
    return sum_valid_ids(puzzle_input=puzzle_input, part=1)

def part2(puzzle_input: str) -> int:
    return sum_valid_ids(puzzle_input=puzzle_input, part=2)


def solve(puzzle_input: str) -> tuple[int, int]:
    part_one = part1(puzzle_input)
    part_two = part2(puzzle_input)

    return part_one, part_two


if __name__ == "__main__":
    run_puzzle(solve_function=solve, example=False)
