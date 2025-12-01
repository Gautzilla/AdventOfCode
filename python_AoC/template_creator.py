from pathlib import Path
from pandas import Timestamp, Timedelta
from typing import Callable


def read_input(example: bool) -> str:
    with open("example_input.txt") as e:
        example_input = e.read()

    with open("puzzle_input.txt") as i:
        puzzle_input = i.read()

    return example_input if example else puzzle_input


def run_puzzle(solve_function: Callable, *, example: bool):
    i = read_input(example=example)

    t_start = Timestamp.now()
    p1, p2 = solve_function(puzzle_input=i)
    t_stop = Timestamp.now()
    t = Timedelta(t_stop - t_start).total_seconds()

    print(f"{'Part one':<20}{p1:>10}")
    print(f"{'Part two':<20}{p2:>10}")

    print(f"\nSolved in {t} second{'s' if t >= 2 else ''}.")


class Solver:
    def __init__(self, year: int, day: int, template: str):
        self.year = year
        self.day = day
        self.template = template

    def __repr__(self):
        return f"{self.year}_{self.day:>02}"

    def to_file(self, year_folder: Path):
        directory = year_folder / f"{self.day:>02}"
        directory.mkdir(parents=True, exist_ok=True)
        open(directory / "example_input.txt", "a").close()
        open(directory / "puzzle_input.txt", "a").close()
        with open(directory / f"{str(self)}.py", "w") as f:
            f.write(self.template)
        print(f"Created {str(self)} in {year_folder}")


def create_solvers(year: int) -> None:
    year_folder_path = Path(f"{Path.cwd()}/{year}")
    with open("puzzle_template.py") as t:
        template = t.read()
    for day in range(1, 13):
        solver = Solver(day=day, year=year, template=template)
        solver.to_file(year_folder=year_folder_path)


if __name__ == "__main__":
    create_solvers(2025)
