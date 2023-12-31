# ExamBankSystem

## 运行要求/编译要求
- 最低版本为`Win10 17763`
- 请在该应用`设置`中`打开数据库`,将[测试数据库](https://github.com/kitUIN/ExamBankSystem/releases/tag/1.0.0.0)放入该文件夹并命名为`bank.sqlite`
- 编译要求:
  - Visual Studio 2020 以上
  - 负载中安装了`.NET 桌面开发`中的`Windos 应用 SDK C# 模板`

### 课设要求
考试题库管理系统
本系统的用户为教师，系统用户分为管理员和普通用户两种。管理员为系统的高级用户，普通用户为题库使用人员。管理员账号和密码预先写入数据库中，账号是每位同学的学号，密码是学号后三位。

系统支持的试题题型包括：单选题、多选题、填空、判断题、简答题、设计题、程序题。试题难易度取值[1,5]，分值越高难度越高。试卷中所有试题的平均难度决定了试卷的难易度。

系统为不同用户提供不同的功能：

- 管理员功能：
  - 登陆：输入管理员账号和密码，验证通过后可登陆系统。  
  - 用户管理
    - [x] 添加系统普通用户。
    - [x] 为普通用户重置登陆密码。
  - 考试科目管理
    - [x] 添加一个新的考试科目（科目名不能重复添加）。
    - [x] 修改考试科目名称（科目名不能重复）。
    - [x] 现有考试科目查询。
    - [x] 删除一个考试科目（仅当该科目下没有题目时）。
  - 知识点管理
    - [x] 添加知识点：向一个考试科目下添加一个新的知识点名称（知识点名称不能重复添加）。
    - [x] 修改知识点名称（知识点名称不能重复）。
    - [x] 知识点查询。
    - [x] 删除一个知识点（仅当该知识点没有关联题目时）。
  - 题目管理
    - [x] 添加单个题目，信息包括：所属科目，题型，题干，答案（包括文字和图片），题目对应的知识点，题目难易度。（同一题型下，相同题干不能重复添加，即题干中重合字符的比例不能高于系统设定的阈值）
    - [x] 批量添加题目：用户选定两个word文档，一个是试卷文档，一个是答案word文档，系统自动提取文档中不同题型的题目和相应的答案，导入数据库。文档格式见参考试卷.doc和答案.doc文档。（同一题型下，相同题干不能重复添加，即题干中重合字符的比例不能高于系统设定的阈值）
    - [x] 修改题目：没有被任何试卷的题目可以进行修改。
    - [x] 删除题目：没有被任何试卷采用的题目可以进行删除。
    - [x] 查询题目：按科目、题型、知识点、难易度等条件进行题目详细信息的查询。
- 普通用户功能：
  - 系统登陆和密码修改
    - [x] 教师凭用户号和密码登陆系统。
    - [x] 用户可以修改自己的密码（须同时输入账号和旧密码进行验证），下一次登陆时使用新密码进行登陆验证。
  - 试卷查询
    - [x] 查询自己出过的试卷信息（试卷名称，试卷生成时间，总分值）。
    - [x] 查询某套试卷对应的全部题目详细信息（题型、题干、答案、分值、难易度、知识点等）。
    - [x] 统计一张试卷中各难度题目的数量及总分数；统计一张试卷中各知识点题目的数量及总分数。
    - [x] 试卷审核：自动生成的试卷需要用户审核，审核通过的试卷不能再修改。
  - 组卷
    - [x] 单张试卷：选择试卷考查的一组知识点、试卷包含的一组题型以及每种题型对应的题目个数，设定试卷难易度，选择不能出现重题的几份往年试卷（试卷word文档），系统自动生成一套试题（该套试题不能与往年试卷中的题目重复，重复的判定标准可以用一道题中重合字符的比例等衡量）。
    - [x] AB试卷：选择试卷考查的一组知识点、试卷包含的一组题型以及每种题型对应的题目个数，设定试卷难易度，选择不能出现重题的几份往年试卷（试卷word文档），系统自动生成两套试题（两套试题中没有重复题目，该套试题不能与往年试卷中的题目重复）。
  - 试卷管理
    - [x] 试卷修改：用户可以替换未审核的试卷中的题目。
    - [x] 试卷删除：用户可以删除已生成的试卷。
    - [x] 试卷导出：将已审核的试卷和答案分别导出为两个word文档格式，保存到用户选定的路径。
    - [x] 试卷查重：用户选择两个试卷word文档，系统自动进行两个文档中重复题目的查找。查重结果分为两种情况：一是同一题型且题目字符完全一样；二是同一题型且重合字符比例大于指定阈值。查重结果需要分上述两种情况呈现给用户，用户可以从题库中选择其他题目进行替换，并重新生成试卷和答案文档。

注意：

1)合理分解系统功能，进行模块设计，优化组员分工，提高开发效率，降低开发成本。

2)针对一门学过的本科计算机专业课构建试题库题目集，题型包括单选题，多选题，填空，判断题，主观题，每个题型40题以上，题目及答案正确。试题库中1-5难易度的题目数量比例为1：2：2：2：1。知识点个数10个以上。

3)按照系统需求说明完成各功能。

4)自行设计试卷中所有试题难易度与试卷难易度之间的计算公式，能根据设定的试卷难易度，自动调整各类难易度题目的选择个数。（例如：试卷难度低，试卷题目集中难度低的题目较多；试卷难度高，试卷题目集中难度高的题目较多；即根据试卷难易度，调整不同难易度题目的比例。
