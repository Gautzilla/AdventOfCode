from template_creator import run_puzzle

def overlaps(range1: list[int], range2: list[int]) -> bool:
    return (range1[0]-1) <= range2[1] and (range1[1]+1) >= range2[0]

def parse_ranges(ranges: set[str]) -> list[list[int]]:
    ranges = sorted([list(map(int,r.split("-"))) for r in ranges], key=lambda r: r[0])
    output = []

    for start, stop in ranges:
        if not output or start > output[-1][1]+1:
            output.append([start,stop])
            continue
        output[-1][1] = max(output[-1][1],stop)
    return output

def part1(ranges: set[str], ingredients:set[int]) -> any:
    ranges = parse_ranges(ranges)
    return sum(1 for ingredient in ingredients if any((ingredient in range(r[0],r[1]+1)) for r in ranges))


def part2(ranges: set[str]) -> any:
    ranges = parse_ranges(ranges)
    return sum(r[1]-r[0]+1 for r in ranges)

def parse_input(puzzle_input: str) -> tuple[set[str],set[int]]:
    ranges, ingredients = set(), set()
    current_list = ranges
    for line in puzzle_input.splitlines():
        if not line:
            current_list = ingredients
            continue
        current_list.add(line)
    return ranges, set(map(int,ingredients))



def solve(puzzle_input: str) -> tuple[any, any]:
    ranges, ingredients = parse_input(puzzle_input)
    part_one = part1(ranges, ingredients)
    part_two = part2(ranges)

    return part_one, part_two


if __name__ == "__main__":
    run_puzzle(solve_function=solve, example=False)
