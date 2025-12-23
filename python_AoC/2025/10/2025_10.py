from template_creator import run_puzzle
from collections import deque

def press_button(diagram: int, button: int) -> int:
    return diagram^button

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

def parse_line(line: str) -> tuple[int, list[int], str]:
    """Parse a manual line, returning the diagram, buttons and joltage requirements."""
    diagram, *buttons, joltage = line.split()
    buttons = [parse_button(button, len(diagram)-2) for button in buttons] # -2 to account for []
    diagram = parse_diagram(diagram)
    primary_buttons = [button for button in buttons if button&diagram] # might switch target lights + extra lights
    buttons = [button for button in buttons if any(b&button for b in primary_buttons)]
    return diagram, buttons, joltage

def parse_input(input: str) -> list[tuple[int, list[int], str]]:
    return list(map(parse_line, input.splitlines()))

def min_nb_switch(target:int, buttons: list[int]) -> int:
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
            next_diagram = press_button(diagram=diagram, button=button)
            queue.append((next_diagram, depth+1))


def part1(input: list[tuple[int, list[int], str]]) -> int:
    return sum(min_nb_switch(target, buttons) for target,buttons,_ in input)


def part2(puzzle_input: str) -> any:
    return ""


def solve(puzzle_input: str) -> tuple[any, any]:
    input = parse_input(puzzle_input)
    part_one = part1(input)
    part_two = part2(puzzle_input)

    return part_one, part_two


if __name__ == "__main__":
    run_puzzle(solve_function=solve, example=False)
