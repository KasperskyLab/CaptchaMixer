# CAPTCHA anatomy

## Image generation pipeline

Image generation pipeline consists of:

- A set of preparatory vector layers
- A set or preparatory raster layers
- Master raster layer

Each **layer** - both vector and raster - is a sequence of **processors**.
A processor is a worker which alters the contents of a layer in some way.

Every preparatory layer has a name.
Those names are used for referencing layers when mixing them in various ways (rasterizing vector layers, copying data between layers, etc.).

Master layer is a normal raster layer with a predefined name `master`.
It is the only required layer within any pipeline.
Contents of this layer define the final CAPTCHA image.

## Vector layers

Vector layer structure from top to bottom:

- Each **vector layer** is a list of **vector objects**
- Each **vector object** is a list of **vector paths**
- Each **vector path** is a list of **vector path instructions**
- **Vector path instructions** may form one or many separate **contours** within a path
- Each **vector path instruction** has a **type** and a fixed-size array of **points** and their _point types_, which may be either a _contour point_ or a Bezier curve _control point_.

Vector instruction types and the way they are treated is similar to what you may see in other vector-related file formats and software.
You may move the pen around, add lines, curves, rectangles, etc.

### Pen position

Vector path instructions describe canvas operations relative to the _current pen position_.
The pen initially has a position of (0, 0).

In order to simplify vector processors development, the `VectorPath` class implements `IEnumerable<(Vector2 position, VectorPathInstruction instruction)>`, so you can iterate over path instructions while seeing the current pen position on each step.

### Building paths

While you may work with the `VectorPath.Instructions` list directly, using shorthand extension methods for appending new instructions in a fluent manner makes your code lightweight and intuitive:

```csharp
var path = new VectorPath()
    .MoveTo(10, 10)
    .LineTo(20, 10)
    .LineTo(20, 20)
    .Close();

var obj = new VectorObject(path);
```

### Contour instructions

Some of the vector instructions are so-called `ContourInstruction`s - these are `AddOvalInstruction`, `AddRectInstruction` and `AddRoundRectInstruction`.
These instructions have a benefit of faster rendering but also have a drawback of worse preparational processing capabilities.

Most vector processors work with points of instructions.
A single contour instruction describes an entire contour using less points.
For example, a rectangle instruction uses just 2 points instead of 4, so when you apply rotation transformation this instruction, the result is still a horizontally-placed rectangle, but with it's anchor points shifted to new positions.
If you use such instructions and need to _fully normally_ process objects/paths which contain them, then use the built-in `DisassembleContours` processor first.

### Importance of composition

Most vector processors operate on a specific abstraction layer of vector data.
When possible, the processor's name directly indicates this, e.g. `RotateObjects`, `PartitionPaths` or `MovePoints`.
But in many cases you will have to rely on code documentation or on trial-and-error, especially when doing something more or less complex.

Errors in composition on lower levels may damage the intended look of vectors.
For example, the letter "O" is normally described as two oval contours - one contained by another.
When combined within a single path, those two ovals will intersect and only the area between them will be filled during rendering.
Breaking those ovals into two different paths would ruin the normal rendering - they will both be completely filled with color.

## Raster layers

Raster layer contents are binary RGBA `byte[]` data.

Since the library is only using single-threaded CPU calculations, processing raster data is sometimes painfully slow, so avoid it when possible.