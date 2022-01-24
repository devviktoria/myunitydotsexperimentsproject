# Emission Masking Experiment
The EmmissionMaskingScene contains 3 cubes that has an emissive material assigned. 
That material has overrides turning on the emission on the back, top and left side of the cube.
You can switch the emission during runtime with B, T and L keys respectively.

## Material setup
In the Shaders directory you can find the CubeShaderGraph, that is used to calculate the emission of the cube.
In the Materials directory you can find the CubeShaderGraphMat, which is a material for the CubeShaderGraph.
The material override asset: CubeShaderGraphMatOverride can be found in the Materials dorectory too. The generated material override script are in the Scripts/MaterialOverrides directory. These the components that are going to be added to those entities that has the CubeShaderGraphMatOverride on them.
(The textures can be found in the Textures directory.)

## The data components
These are located in the Scripts/Components directory.
The RequestEmissionSwitchInputData contains the input keycodes for requesting a switch on the cube emission.
The RequestEmissionSwitchData contains the booleans of which switches were requested.

## The systems
These are located in the Scripts/Systems directory.
The RequestEmissionSwitchSystem is processing the input from the user. This uses the old input manager for simplicity.
The EmissionSwitchSystem does the emission switch by manipulating the material overrides.

