from template_creator import run_puzzle
from collections import deque
import z3

def press_light_button(diagram: int, button: int) -> int:
    return diagram^button

def press_joltage_button(joltages: list[int], button:int) -> list[int]:
    output = [*joltages]
    for index in range(len(joltages)):
        if 2**index & button:
            output[len(output)-index-1] +=1
    return output

def parse_diagram(diagram: str) -> int:
    """Return the lit lights in the form of a base2 int."""
    diagram = diagram.lstrip("[").rstrip("]")
    diagram = diagram.replace(".","0").replace("#","1")
    return int(diagram,2)

def parse_button(button: str, diagram_length: int) -> int:
    """Return a base 2 mask from the button description"""
    button = button.lstrip("(").rstrip(")")
    coords = list(map(int,button.split(",")))
    return sum(2**(diagram_length-c-1) for c in coords)

def parse_line(line: str) -> tuple[int, list[int], list[int]]:
    """Parse a manual line, returning the diagram, buttons and joltage requirements."""
    diagram, *buttons, joltage = line.split()
    buttons = [parse_button(button, len(diagram)-2) for button in buttons] # -2 to account for []
    diagram = parse_diagram(diagram)
    joltage = list(map(int,joltage.lstrip("{").rstrip("}").split(",")))
    return diagram, buttons, joltage

def parse_input(input: str) -> list[tuple[int, list[int], list[int]]]:
    return list(map(parse_line, input.splitlines()))

def min_nb_light_switch(target:int, buttons: list[int]) -> int:
    queue = deque([(0, 0)]) # status, depth, visited
    visited = set()

    while queue:
        diagram, depth = queue.popleft()
        if diagram == target:
            return depth
        if diagram in visited:
            continue
        visited.add(diagram)
        for button in buttons:
            next_diagram = press_light_button(diagram=diagram, button=button)
            queue.append((next_diagram, depth+1))

def solve_joltage(buttons: list[int], joltages: list[int]) -> int:
    o = z3.Optimize()
    nb_presses = z3.Ints(f"n{i}" for i in range(len(buttons)))
    for press in nb_presses:
        o.add(press >= 0)
    for idx, joltage in enumerate(joltages[::-1]):
        equation = 0
        for b_idx, button in enumerate(buttons):
            if not (2 ** idx & button):
                continue
            equation += nb_presses[b_idx]
        o.add(equation == joltage)
    o.minimize(sum(nb_presses))
    o.check()
    return o.model().eval(sum(nb_presses)).as_long()

def part1(input: list[tuple[int, list[int], list[int]]]) -> int:
    return sum(min_nb_light_switch(target, buttons) for target,buttons,_ in input)


def part2(input: list[tuple[int, list[int], list[int]]]) -> any:
    return sum(solve_joltage(buttons, joltages) for _,buttons,joltages in input)


def solve(puzzle_input: str) -> tuple[any, any]:
    input = parse_input(puzzle_input)
    part_one = part1(input)
    part_two = part2(input)

    return part_one, part_two


if __name__ == "__main__":
    run_puzzle(solve_function=solve, example=False)
