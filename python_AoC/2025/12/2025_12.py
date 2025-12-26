from template_creator import run_puzzle


def parse_shapes(puzzle_input: str) -> list[set[tuple[int,int]]]:
    shapes = []
    current_shape = set()
    x=0
    for line in puzzle_input.splitlines():
        if "x" in line:
            break
        if not line:
            shapes.append(current_shape)
            current_shape = set()
        if ":" in line:
            x=0
            continue
        for y,c in enumerate(line):
            if c == ".":
                continue
            current_shape.add((x,y))
        x+=1
    return shapes

def parse_regions(puzzle_input: str) -> list[tuple[tuple[int,int], list[int]]]:
    regions = []
    for line in puzzle_input.splitlines():
        if not "x" in line:
            continue
        dimensions, presents = line.split(":", maxsplit=1)
        dimensions = tuple(map(int,dimensions.split("x", maxsplit=1)))
        presents = list(map(int, presents.split()))
        regions.append((dimensions, presents))
    return regions

def max_sizes(nb_shapes: list[int]) -> int:
    return sum(3*3*nb_shape for nb_shape in nb_shapes)

def min_sizes(shapes: list[set[tuple[int,int]]], nb_shapes: list[int]) -> int:
    return sum(len(shape)*nb_shape for shape,nb_shape in zip(shapes, nb_shapes, strict=True))

def is_fit_obvious(region: tuple[tuple[int,int], list[int]]) -> bool:
    dimensions, nb_shapes = region
    size = dimensions[0]*dimensions[1]
    return size >= max_sizes(nb_shapes)

def is_fit_impossible(region: tuple[tuple[int,int], list[int]], shapes: list[set[tuple[int,int]]]) -> bool:
    dimensions, nb_shapes = region
    size = dimensions[0]*dimensions[1]
    return min_sizes(shapes, nb_shapes) > size

def solve(puzzle_input: str) -> tuple[any, any]:
    shapes = parse_shapes(puzzle_input)
    regions = parse_regions(puzzle_input)
    r_fit = [region for region in regions if is_fit_obvious(region)]
    r_no_fit = [region for region in regions if is_fit_impossible(region, shapes)]
    r_undetermined = [region for region in regions if (not region in r_fit and not region in r_no_fit)]
    if r_undetermined:
        raise ValueError("Input has undetermined regions.")
    part_one = len(r_fit)
    part_two = 0

    return part_one, part_two


if __name__ == "__main__":
    run_puzzle(solve_function=solve, example=False)
