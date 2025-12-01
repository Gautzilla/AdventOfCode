from template_creator import run_puzzle
from enum import Enum
import numpy as np
import re


class Action(Enum):
    TURNON = 1
    TURNOFF = 2
    TOGGLE = 3


class Instruction:
    def __init__(
        self, action: Action, top_left: tuple[int, int], bottom_right: tuple[int, int]
    ):
        self.action = action
        self.top_left = top_left
        self.bottom_right = bottom_right

    actions = {
        "turn on": Action.TURNON,
        "turn off": Action.TURNOFF,
        "toggle": Action.TOGGLE,
    }

    @classmethod
    def from_str(cls, s: str):
        match = re.search(
            r"(?P<action>\D+)(?P<top_left>\d+,\d+) through (?P<bottom_right>\d+,\d+)", s
        )
        action = cls.actions[match.group("action").strip()]
        top_left = tuple(int(p) for p in match.group("top_left").split(","))
        top_right = tuple(int(p) for p in match.group("bottom_right").split(","))
        return cls(action, top_left, top_right)


class Square:
    def __init__(self, size_x: int, size_y: int, part: int):
        self.lights = np.zeros((size_x, size_y))
        self.part = part

    light_actions = {
        Action.TURNON: lambda light: 1,
        Action.TURNOFF: lambda light: 0,
        Action.TOGGLE: lambda light: (light - 1) ** 2,
    }

    light_actions_part_2 = {
        Action.TURNON: lambda light: light + 1,
        Action.TURNOFF: lambda light: np.maximum(light - 1, 0),
        Action.TOGGLE: lambda light: light + 2,
    }

    def activate(self, instruction: Instruction) -> None:
        top_left = instruction.top_left
        bottom_right = instruction.bottom_right
        action = instruction.action
        a, b, c, d = top_left[0], bottom_right[0] + 1, top_left[1], bottom_right[1] + 1
        light_action = (
            self.light_actions[action]
            if self.part == 1
            else self.light_actions_part_2[action]
        )
        self.lights[a:b, c:d] = light_action(self.lights[a:b, c:d])

    def lit_lights(self) -> int:
        return int(np.sum(self.lights))


def part1(instructions: list[Instruction]) -> any:
    sq = Square(1_000, 1_000, part=1)
    for i in instructions:
        sq.activate(i)
    return sq.lit_lights()


def part2(instructions: list[Instruction]) -> any:
    sq = Square(1_000, 1_000, part=2)
    for i in instructions:
        sq.activate(i)
    return sq.lit_lights()


def solve(puzzle_input: str) -> tuple[any, any]:
    instructions = [Instruction.from_str(s) for s in puzzle_input.splitlines()]
    part_one = part1(instructions)
    part_two = part2(instructions)

    return part_one, part_two


if __name__ == "__main__":
    run_puzzle(solve_function=solve, example=True)
