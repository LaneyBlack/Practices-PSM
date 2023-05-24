import turtle

n = 5
start_instruction = "X"
x_formula = "F+[[X]-X]-F[-FX]+X"  # X -> F+[[X]-X]-F[-FX]+X
f_formula = "FF"  # F -> FF


class Screen:
    def __init__(self):
        # Init window
        self.window = turtle.Screen()
        self.window.title("Growth Simulation")
        self.window.bgcolor("black")
        # Init brush (turtle)
        self.brush = turtle.Turtle()
        self.brush.color("green")
        self.brush.shape("classic")
        self.brush.speed(0)
        # self.brush.shapesize(stretch_wid=0.5)
        # Put brush into start pos
        self.brush.penup()
        self.brush.goto(0, -self.window.window_height() / 2)
        self.brush.left(90)  # turn upside
        self.brush.pendown()


def get_instructions(instructions, x_formula, f_formula, n):
    for i in range(n):
        instructions = instructions.replace("F", f_formula)
        instructions = instructions.replace("X", x_formula)
    return instructions


def draw_instructions(instructions, screen):
    stack = []
    for character in instructions:
        if character == 'F':
            screen.brush.forward(10)
        elif character == '+':
            screen.brush.left(25)
        elif character == '-':
            screen.brush.right(25)
        elif character == '[':
            coordinates = [screen.brush.xcor(), screen.brush.ycor()]
            angle = screen.brush.heading()
            stack.append((angle, coordinates))
        elif character == ']':
            angle, coordinates = stack.pop()
            screen.brush.penup()
            screen.brush.setheading(angle)
            screen.brush.goto(coordinates[0], coordinates[1])
            screen.brush.pendown()
        elif character == 'X':
            screen.brush.forward(0)  # 0 or 25
    screen.window.exitonclick()


def main():
    screen = Screen()
    instructions = get_instructions(start_instruction, x_formula, f_formula, n)
    draw_instructions(instructions, screen)


if __name__ == "__main__":
    main()
