#pragma once

using namespace std;

const int n = 3;

class CSM
{
public:
	CSM(int w, int h, unsigned int *_depthMap, unsigned int *_depthMapFBO);
private:
	int SH_WIDTH;
	int SH_HEIGHT;
};