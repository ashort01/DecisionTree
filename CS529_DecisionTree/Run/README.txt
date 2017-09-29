Decision Tree run instructions.

From this directory in a windows command line prompt run:

./DecisionTree.exe --a 0.02 --d information-gain --p .7 --s

This is an example run with specific parameters.

Parameters:
a is alpha value for chi squared
d is decision algorithm
p is purity ratio
s is show the final tree

Possible values for paramaters are: 
a: 0.995, 0.975, 0.20, 0.10, 0.05, 0.025, 0.02, 0.01, 0.005, 0.002, 0.001
d: information-gain, gini-index, both
p: any decimal number between 0 and 1
s: either include it or not
