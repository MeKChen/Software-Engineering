#include <iostream>
#include <fstream>
#include <algorithm>
#include <cstdio>
#include <string>
#include <vector>
#include "json//json.h"
#pragma comment(lib, "json_vc71_libmtd.lib")    
using namespace std;
const int stumaxn = 5001;
const int deptmaxn = 101;
typedef pair<string, string> pii;
vector<pii> v;

struct Student {
	string stu_no;
	double gpa;
	vector<string>dept_no;
	vector<string>tags;
	int pos;
	bool pass;
	int bechoosen;
	int flag;
	double stupoint;
}stu[stumaxn];

struct Dept {
	string dept_no;
	int maxnum;
	int passnum;
	vector<string>tags;
	vector<string>stu_no;
	int choose;
}dept[deptmaxn];

int tagsnum(Student stu, Dept dept)
{
	int tagnum = 0;
	int depttags = dept.tags.size();
	int stutags = stu.tags.size();
	for (int i = 0; i < stutags; i++)
	{
		for (int j = 0; j < depttags; j++)
		{
			if (stu.tags[i] == dept.tags[j])
			{
				tagnum++;
				break;
			}
		}
	}
	return tagnum;
}

int deptnum(string str, int size)
{
	int index;
	for (int i = 0; i < size; i++)
	{
		if (str == dept[i].dept_no)
		{
			index = i;
			break;
		}
	}
	return index;
}

int stunum(string str, int size)
{
	int index;
	for (int i = 0; i < size; i++)
	{
		if (str == stu[i].stu_no)
		{
			index = i;
			break;
		}
	}
	return index;
}

//部门对学生打分
double score(Student stu, Dept dept, int pos)
{
	double points = 0;
	points = points + 20 - (5 * pos);
	points = points + stu.gpa * 5;
	int tag = tagsnum(stu, dept);
	points = points + 6 * (tag > 5 ? 5 : tag);
	return points;
}

//分数的存储
void getscore(int ssize, int dsize)
{
	for (int i = 0; i < ssize; i++)
	{
		int dept_no_size = stu[i].dept_no.size();
		for (int j = 0; j < dept_no_size; j++)
		{
			int t = deptnum(stu[i].dept_no[j], dsize);
			stu[i].stupoint = score(stu[i], dept[t], j);
		}
	}
}

struct Node
{
	int stu_index;
	double point;
	bool operator < (const Node& r) const
	{
		return point > r.point;
	}
} list[stumaxn];

bool cmp(Student a, Student b)
{
	return a.stupoint > b.stupoint;
}

void matching(int ssize, int dsize)
{
	for (int t = 0; t < dsize; t++)
	{
		//dl(t);
		int total = 0;
		for (int i = 0; i < ssize; i++)
		{
			for (int k = 0; k < stu[i].dept_no.size(); k++)
				if (deptnum(stu[i].dept_no[k], dsize) == t && stu[i].flag == 0)
				{
					list[total].stu_index = i;
					list[total].point = stu[i].stupoint;
					total++;
				}
		}
		sort(list, list + total);
		for (int i = 0; i < dept[t].maxnum && i < total; i++)
		{
			int stu_index = list[i].stu_index;
			v.push_back(pii(dept[t].dept_no, stu[stu_index].stu_no));
			dept[t].choose = 1;
			stu[stu_index].bechoosen = 1;
		}
	}
}

//对输入文件的各种数据的解析
void parseDepno(Json::Value& root, int i)
{
	string department_no = root["departments"][i]["department_no"].asString();
	dept[i].dept_no = department_no;
	dept[i].choose = 0;
}

void parseLimit(Json::Value& root, int i)
{
	int deplimit = root["departments"][i]["member_limit"].asInt();
	dept[i].maxnum = deplimit;
	dept[i].passnum = 0;
}

void parseDtags(Json::Value& root, int i)
{
	Json::Value dtags = root["departments"][i]["tags"];
	int sizeofdtags = dtags.size();
	for (int j = 0; j < sizeofdtags; j++)
	{
		string str = dtags[j].asString();
		dept[i].tags.push_back(str);
	}
}

void parseStuno(Json::Value& root, int i)
{
	string stu_no = root["students"][i]["student_no"].asString();
	stu[i].stu_no = stu_no;
	stu[i].bechoosen = 0;
}

void parseGpa(Json::Value& root, int i)
{
	double gpa = root["students"][i]["gpa"].asDouble();
	stu[i].gpa = gpa;
}

void parseStags(Json::Value& root, int i)
{
	Json::Value stags = root["students"][i]["tags"];
	int sizeofstags = stags.size();
	for (int j = 0; j < sizeofstags; j++)
	{
		string str = stags[j].asString();
		stu[i].tags.push_back(str);
	}
}

void parseStuDept(Json::Value& root, int i)
{
	Json::Value adep = root["students"][i]["available_dep"];
	int sizeofadep = adep.size();
	for (int j = 0; j < sizeofadep; j++)
	{
		string str = adep[j].asString();
		stu[i].dept_no.push_back(str);
	}
}

int main()
{
	Json::Reader reader;
	Json::Value inroot;

	int ssize;
	int dsize;

	ifstream in("input.txt", ios::binary);
	ofstream fs;
	fs.open("output_data.txt");
	//cout << 1 << endl;

	if (reader.parse(in, inroot))
	{
		ssize = inroot["students"].size();
		dsize = inroot["departments"].size();

		for (int i = 0; i < dsize; i++)
		{
			parseDepno(inroot, i);
			parseLimit(inroot, i);
			parseDtags(inroot, i);
		}

		for (int i = 0; i < ssize; i++)
		{
			parseStuno(inroot, i);
			parseStuDept(inroot, i);
			parseStags(inroot, i);
			parseGpa(inroot, i);
		}
	}

	getscore(ssize, dsize);
	matching(ssize, dsize);

	int sz = v.size();

	//输出
	Json::Value root;

	//未被招收的学生
	for (int i = 0; i < ssize; i++)
	{
		if (stu[i].bechoosen == 0)
		{
			root["standalone_students"].append(stu[i].stu_no);
		}
	}

	Json::Value matched;
	for (int t = 0; t < sz; t++)
	{
		Json::Value matched;
		matched["department_no"].append(v[t].first);
		matched["student_no"].append(v[t].second);
		root["matched_departmentno_studentno"].append(matched);

	}

	//未招到人的部门
	for (int i = 0; i < dsize; i++)
	{
		if (dept[i].choose == 0)
		{
			root["standalone_departments"].append(dept[i].dept_no);
		}
	}

	Json::StyledWriter writer;
	string s = writer.write(root);
	fs << s;

	in.close();
	return 0;
}