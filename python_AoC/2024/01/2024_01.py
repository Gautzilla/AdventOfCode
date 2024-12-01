from template_creator import read_input
from pandas import Timestamp, Timedelta

def solve() -> tuple[int, int]:
    lines = [[int(v) for v in line.split()] for line in i.split("\n")]
    list_one = sorted(line[0] for line in lines)
    list_two = sorted(line[1] for line in lines)

    part_one = sum(abs(c[1] - c[0]) for c in zip(list_one, list_two))
    part_two = sum(v * sum(1 for i in list_two if i == v) for v in list_one)

    return part_one, part_two

if __name__ == "__main__":
    i = read_input(example = False)

    t_start = Timestamp.now()
    p1, p2 = solve()
    t_stop = Timestamp.now()
    t = Timedelta(t_stop - t_start).total_seconds()

    print(f"{'Part one':<20}{p1:>10}")
    print(f"{'Part two':<20}{p2:>10}")
    print(f"\nSolved in {t} second.")