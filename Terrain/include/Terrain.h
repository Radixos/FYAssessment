#ifndef TERRAIN_H
#define TERRAIN_H


#include <vector>
#include <glad/glad.h>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <iostream>

#include "PerlinNoise.h"

class Terrain
{
public:
	Terrain(int widthIn, int heightIn, int stepSizeIn);
	Terrain();
	std::vector<float> getVertices();
	PerlinNoise perlin;
	float seed;
private:
	std::vector<float> vertices;
	int width;
	int height;
	int stepSize;
	void makeVertices(std::vector<float> *vertices);
	void makeVertex(int x, int y, std::vector<float> *vertices);
	double cycleOctaves(glm::vec3 pos, int numOctaves);
};
#endif












