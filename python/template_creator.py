from pathlib import Path
from pandas import Timestamp

class Solver:
    def __init__(self, year: int, day: int):
        self.year = year
        self.day = day

    SOLVER_TEMPLATE = 'with open("example_input.txt") as e:\n\texample_input = e\n\nwith open("puzzle_input.txt") as i:\n\tpuzzle_input = i'

    def __repr__(self):
        return f"{self.year}_{self.day:>02}"

    def to_file(self, year_folder: Path):
        directory = year_folder / f"{self.day:>02}"
        directory.mkdir(parents = True, exist_ok=True)
        with open(directory/"example_input.txt", "w") as e:
            e.write("")
        with open(directory/"puzzle_input.txt", "w") as i:
            i.write("")
        with open(directory / f"{str(self)}.py", "w") as f:
            f.write(self.SOLVER_TEMPLATE)
        print(f"Created {str(self)} in {year_folder}")

def create_solvers(year: int) -> None:
    year_folder_path = Path(f"{Path.cwd()}/{year}")
    for day in range(1,26):
        solver = Solver(day = day, year = year)
        solver.to_file(year_folder = year_folder_path)

if __name__ == "__main__":
    create_solvers(Timestamp.today().year)