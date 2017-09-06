#include<stdio.h>
#include<cstdlib>
#include<time.h>

int data[9][9];
int first[9] = { 1,2,3,4,5,6,7,8,9 };

void rd(int a[])
{
	int temp, random;
	for (int i = 0; i <= 8; i++)
	{
		random = rand() % 9;
		temp = a[random];
		a[random] = a[i];
		a[i] = temp;
	}
}

//判断函数
bool judge(int x, int y, int q)
{
	int i, j;
	//判断行
	for (i = x, j = 0; j < y; j++)
	{
		if (q == data[i][j])
			return false;
	}
	//判断列
	for (i = 0, j = y; i < x; i++)
	{
		if (q == data[i][j])
			return false;
	}
	//判断九宫格
	int count = y % 3 + x % 3 * 3;
	while (count--)
	{
		if (q == data[x - x % 3 + count / 3][j - j % 3 + count % 3])
			return false;
	}
	return true;
}

//填数函数，递归
bool fill(int i, int j)
{
	int s, k, p;
	if (i > 8)
		return true;
	s = 1 + rand() % 9;               //使其填数不唯一
	for (k = 9; k > 0; k--)
	{
		p = (s + k) % 9 + 1;
		if (judge(i, j, p))
		{
			data[i][j] = p;
			if (j == 8)
			{
				i++;
				j = 0;
			}
			else
			{
				j++;
			}
			if (fill(i, j))
				return true;
		}
	}
	return false;
}

int main()
{
	FILE *p;
	p = fopen("sudoku.txt", "w");
	int z, n, flag;

	while (!scanf("%d", &n))
	{
		rewind(stdin);
		printf("输入错误，请重新输入：\n");
	}



	for (z = 0; z < n; z++)
	{
		int i = 0, j = 0;
		for (i = 0; i < 9; i++)
			for (j = 0; j < 9; j++)
			{
				data[i][j] = 0;            //数独盘初始化为零
			}
		srand(time(NULL));
		rd(first);

		for (i = 0; i < 9; i++)
		{
			if (first[i] == 5)
			{
				flag = first[i];
				first[i] = first[0];
				first[0] = flag;
			}
		}

		for (j = 0; j < 9; j++)
		{
			data[0][j] = first[j];
		}

		fill(1, 0);

		for (i = 0; i < 9; i++)            //打印数独终盘
		{
			for (j = 0; j < 9; j++)
			{
				//printf("%d ", data[i][j]);
				fprintf(p, "%d ", data[i][j]);
			}
			//printf("\n");
			fprintf(p, "\n");
		}
		//printf("\n");
		fprintf(p, "\n");
	}
	fclose(p);
	system("pause");
	return 0;
}
