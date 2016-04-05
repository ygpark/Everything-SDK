
// Everything IPC test
// this tests the lib and the dll.

#include <stdio.h>

#include "..\include\Everything.h"

int main(int argc,char **argv)
{
	Everything_SetSearchW(L"hello");
	Everything_QueryW(TRUE);
	
	{
		DWORD i;
		
		for(i=0;i<Everything_GetNumResults();i++)
		{
			printf("%S\n",Everything_GetResultFileNameW(i));
		}
	}
	
	return 0;
}
