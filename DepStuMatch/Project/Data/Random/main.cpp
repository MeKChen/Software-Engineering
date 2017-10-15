#include <iostream>
#include <string>
#include <cstdio>
#include <time.h> 
#include <sstream>
#include <cstdlib>
#include <vector>
#include <algorithm>
#include <fstream>
#include "json//json.h"
#pragma comment(lib, "json_vc71_libmtd.lib")    
using namespace std;

string tags[] = {
	"runing", "writing","reading", "film", "painting", "reading","basketball","dancing","singing","guitar","football"
};

string weeks[] = { 
	"Mon.","Tues.","Wed.","Thurs.","Fri.","Sat.","Sun." 
};

string sche[] = {
	"10:00-12:00","13:00-15:00","15:00-17:00","18:00-20:00","20:00-22:00"
};

struct Department
{
	string department_no;
	int member_limit;
	int tnum;
	int timenum;
	string tags[11];
	string event_schedules[5];
};

struct Student
{
	string student_no;
	int tnum;
	int timenum;
	int count;     //ѡ���뱨���Ĳ���ʱѡ��Ĳ��Ÿ���
	double grade;
	string tags[11];
	string available_dep[5];
	string available_schedules[5];
};

//������ɲ�����Ϣ
void GeD(int Dnum,Department *dep)
{
	int i, j, t;
	string k;
	for (i = 0; i < Dnum; i++)
	{
		//���ɲ��ź�
		t = rand() % 11;
		if (i < 10)
		{
			stringstream ss;
			ss << i;
			ss >> k;
			dep[i].department_no = "D00" + k;
		}
		else if(i >= 10 && i < 100)
		{
			stringstream ss;
			ss << i;
			ss >> k;
			dep[i].department_no = "D0" + k;
		}
		else
		{
			stringstream ss;
			ss << i;
			ss >> k;
			dep[i].department_no = "D" + k;
		}
		//dep[i].department_no = "D00";
		dep[i].member_limit = 1 + rand() % 15;  //����������������
		dep[i].tnum = 1 + rand() % 11;			//������ɴ˲���tag������
		//tag������
		for(j = 0; j < dep[i].tnum ; j++)
		{
			if (t == 10)
				t = 0;
			else
				t++;
			dep[i].tags[j] = tags[t];
		}
		//���Żʱ��ε�����
		dep[i].timenum = 1 + rand() % 3;       //������ɴ˲��Żʱ�������
		for (j = 0; j < dep[i].timenum; j++)
		{
			int flag1, flag2;
			flag1 = rand() % 7;
			flag2 = rand() % 5;
			dep[i].event_schedules[j] = weeks[flag1] + sche[flag2];
		}

	}
}

//����ѧ������
void GeS(int Snum, int Dnum, Student *stu)
{
	int i, j, t;
	string k;
	for (i = 0; i < Snum; i++)
	{
		//����ѧ��
		if (i < 10)
		{
			stringstream ss;
			ss << i;
			ss >> k;
			stu[i].student_no = "S000" + k;
		}
		else if (i >= 10 && i < 100)
		{
			stringstream ss;
			ss << i;
			ss >> k;
			stu[i].student_no = "S00" + k;
		}
		else if (i >= 100 && i < 1000)
		{
			stringstream ss;
			ss << i;
			ss >> k;
			stu[i].student_no = "S0" + k;
		}
		else
		{
			stringstream ss;
			ss << i;
			ss >> k;
			stu[i].student_no = "S" + k;
		}
		stu[i].grade = (rand() % 10) / 10.0 + (rand() % 10);     //���ɼ���
		//�����뱨���Ĳ��ţ�����ͬ
		stu[i].count = 1 + rand() % 5;
		for (j = 0; j < stu[i].count; j++)
		{
			string k1;
			int temp = rand() % Dnum;
			if (temp < 10)
			{
				stringstream ss1;
				ss1 << temp;
				ss1 >> k1;
				stu[i].available_dep[j] = "D00" + k1;
			}
			else if (temp >= 10 && temp < 100)
			{
				stringstream ss1;
				ss1 << temp;
				ss1 >> k1;
				stu[i].available_dep[j] = "D0" + k1;
			}
			else
			{
				stringstream ss1;
				ss1 << temp;
				ss1 >> k1;
				stu[i].available_dep[j] = "D" + k1;
			}
		}
		//����tags
		stu[i].tnum = 1 + rand() % 11;
		t = rand() % 11;
		for (j = 0; j < stu[i].tnum; j++)
		{
			if (t == 10)
				t = 0;
			else
				t++;
			stu[i].tags[j] = tags[t];
		}
		//ѧ������ʱ�������
		stu[i].timenum = 1 + rand() % 5;
		for (j = 0; j < stu[i].timenum; j++)
		{
			int flag1, flag2;
			flag1 = rand() % 7;
			flag2 = rand() % 5;
			stu[i].available_schedules[j] = weeks[flag1] + sche[flag2];
		}
	}
}

Department dep[101];
Student stu[5001];

int main()
{
	srand((int)time(0));
	Json::Value root;
	Json::Value D;
	Json::Value S;
	Json::Value depa;
	Json::Value stud;
	
	int n, m, i, j;
	cin >> n >> m;  //����ѧ�����Ͳ�����

	GeD(m, dep);
	GeS(n, m, stu);
	 
	for (i = 0; i < m; i++)
	{	
		depa["department_no"] = dep[i].department_no;
		depa["member_limit"] = dep[i].member_limit;
		Json::Value Dtags;
		for (j = 0; j < dep[i].tnum; j++)
		{
			Dtags.append(dep[i].tags[j]);
		}
		depa["tags"] = Dtags;
		Json::Value dsche;
		for (j = 0; j < dep[i].timenum; j++)
		{
			dsche.append(dep[i].event_schedules[j]);
		}
		depa["event_schedules"] = dsche;
		D.append(depa);
	}

	for (i = 0; i < n; i++)
	{	
		stud["student_no"] = stu[i].student_no;
		stud["gpa"] = stu[i].grade;
		Json::Value Stags;
		for (j = 0; j < stu[i].tnum; j++)
		{
			Stags.append(stu[i].tags[j]);
		}
		stud["tags"] = Stags;
		Json::Value ssche;
		for (j = 0; j < stu[i].timenum; j++)
		{
			ssche.append(stu[i].available_schedules[j]);
		}
		stud["available_schedules"] = ssche;
		Json::Value sdep;
		for (j = 0; j < stu[i].count; j++)
		{
			sdep.append(stu[i].available_dep[j]);
		}
		stud["available_dep"] = sdep;
		S.append(stud);
	}

	root["departments"] = D;
	root["students"] = S;

	Json::StyledWriter writer;
	string s = writer.write(root);

	ofstream fs;
	fs.open("./input.txt", std::ios::binary);
	fs << s;
	fs.close();

	//system("pause");
	return 0;
}