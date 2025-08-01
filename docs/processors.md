# Processors

Many processors are available out of the box.
See in-code documentation of each class and its properties for more details on usage.

## Vector processors

- Objects creation:
  - Noise: `RandomCurves`, `RandomLines`, `RandomOvals`, `RandomRects`
  - Paths (more intellectual that pure random): `PointsLinePaths`, `SquirmingLinePaths`
  - Primitives (just one object): `Oval`, `Rect`
  - Grids: `LinesGridX`, `LinesGridY`, `OvalsGrid`, `RectsGrid`
  - Oscillations: `CurveOscillation`, `SawOscillation`, `SquareOscillation`, `TriangleOscillation`
  - Segmentations (split area on adjacent parts): `CurveSegmentationX`, `CurveSegmentationY`, `LineSegmentationX`, `LineSegmentationY`
  - Text: `CaptchaChars`, `SkeletonizedCaptchaChars`
- Transformations (affect coordinates of objects' points): `AddSizeObjects`, `JustifyObjectsX`, `JustifyObjectsY`, `MoveObjects`, `MovePoints`, `PopulateObjectsX`, `PopulateObjectsY`, `ResizeObjects`, `RotateObjects`, `RoundPoints`, `ScaleObjects`, `SnapObjects`
- Structural processors (affect composition of vector objects or their internals): `CopyVectorLayer`, `DisassembleContours`, `GranulatePaths`, `MergeObjects`, `MergePaths`, `PartitionPaths`, `SplitObjects`, `SplitPaths`, `ToLines`, `UnclosePaths`
- Distortions: `ChopPaths`, `DamagePaths`, `PointsRadialDistortion`
- Convertions (change paths in complex manner): `PrimitivizePaths`, `StraightenPaths`
- Composites and proxies (organize the work of other processors): `VectorProcessorsIsolatedSequence`, `VectorProcessorsPerObjectSequence`, `VectorProcessorsSelector`, `VectorProcessorsSequence`, `PngExportingVectorProcessorProxy`, `ProbabilityVectorProcessorProxy`

## Raster processors

- Coloring: `AddColors`, `Fill`, `GrayscaleColors`, `HueShift`, `InvertColors`, `SetPixels`
- Effects: `Blur`
- Vectors rendering: `DrawVectorLayer`, `DrawVectorLayersShuffled`
- Rasters mixing: `ApplyMask`, `CopyRasterLayer`, `DrawRasterLayer`, 
- Composites and proxies (organize the work of other processors): `RasterProcessorsIsolatedSequence`, `RasterProcessorsSelector`, `RasterProcessorsSequence`, `PngExportingRasterProcessorProxy`, `ProbabilityRasterProcessorProxy`

## Custom processors

OOB set is enough to create a wide range of visual appearances, but you may need something more specific.

To create a vector processor either implement the `IVectorProcessor` interface or inherit from one of abstract classes provided by the library for quicker start:

- `OneByOneVectorObjectsProcessor`
- `OneByOnePathsProcessor`
- `OneByOnePathInstructionsProcessor`
- `AreaBasedVectorObjectsCreator`
- `PatternCreator`
- `PathCreator`
- `AreaSegmentationCreator`
- `SKFontTextCreator`
- `BasePointObjectsTransformer`
- `VectorProcessorsComposite`

Same for raster processors - implement the `IRasterProcessor` interface or inherit from one of base abstract classes:

- `SKBitmapRasterProcessor`
- `RasterLayerMixer`
- `RasterProcessorsComposite`