# 第二课堂API接口文档

## 登录

### 公钥请求

>  https://erke.smxpt.cn/lencon/app/getPublicKey

请求方式：POST

认证方式：无

**URL参数：**

无...

**jsno回复：**

根对象：

|      字段       | 类型 |  内容  |  备注   |
| :-------------: | :--: | :----: | :-----: |
|      code       | num  | 返回值 | 1：成功 |
| publicKeyString | str  |  公钥  |         |

### 登录请求

> https://erke.smxpt.cn/lencon/app/login 

请求方式：POST

认证方式：无

**正文参数( application/x-www-form-urlencoded;charset=UTF-8 )：**

|    字段    | 类型 |      内容      | 备注 |
| :--------: | :--: | :------------: | :--: |
|  password  | str  |  加密后的密码  |      |
| login_name | str  | 加密后的用户名 |      |
| publicKey  | str  |    加密公钥    |      |

**json回复：**

根对象：

|  字段  | 类型 |   内容   |  备注   |
| :----: | :--: | :------: | :-----: |
|  code  | num  |  返回值  | 1：成功 |
|  msg   | str  | 提示信息 |         |
| token  | str  |   令牌   |         |
| userId | sum  |  用户ID  |         |

## 信息

### 基本信息

> https://erke.smxpt.cn/lencon/app/person/get_info 

请求方式：POST

认证方式：Token

**正文参数( application/x-www-form-urlencoded;charset=UTF-8 )：**

| 字段  | 类型 | 内容 | 备注 |
| :---: | :--: | :--: | :--: |
| token | str  | 令牌 |      |

**json回复：**

根对象：

| 字段 | 类型 |   内容   |  备注   |
| :--: | :--: | :------: | :-----: |
| code | num  |  返回值  | 1：成功 |
| msg  | str  | 提示信息 |         |
| res  | obj  | 信息本体 |         |

`res`对象：

|      字段       | 类型 |       内容       |    备注    |
| :-------------: | :--: | :--------------: | :--------: |
|   banji_name    | str  |       班级       |            |
|  zhuanye_name   | str  |       专业       |            |
|    id_number    | str  |     身份证号     |            |
|   xueyuan_id    | str  |      学院ID      |            |
|    s_number     | str  |       学号       |            |
|       sex       | str  |       性别       |            |
|    banji_id     | str  |      班级ID      |            |
|   phonenumber   | str  |     联系电话     |            |
|   is_migrant    | str  |     是否团干     |            |
|     avatar      | str  |     头像地址     |  相对路径  |
|   ruxue_date    | str  |     入学时间     |  YYYY-MM   |
|   recordDept    | str  |     团员编号     |            |
|     xuezhi      | str  |       学制       |            |
|     is_flow     | str  |     是否团干     |            |
|     tw_name     | str  |     支部名称     |            |
| is_leaguemember | str  |  团关系是否转接  |            |
|      name       | str  |       姓名       |            |
| entryPartyTime  | str  |     入团时间     | YYYY-MM-DD |
|   zhuanye_id    | str  |      专业ID      |            |
|    partyOrg     | str  | 是否录入智慧团建 |            |
|   erke_count    | num  |      <未知>      |            |
|  xueyuan_name   | str  |     学院名称     |            |
|                 |      |                  |            |

### 团组织信息

>  https://erke.smxpt.cn/lencon/app/person/get_tw_info 

请求方式：POST

认证方式：Token

**正文参数( application/x-www-form-urlencoded;charset=UTF-8 )：**

| 字段  | 类型 | 内容 | 备注 |
| :---: | :--: | :--: | :--: |
| token | str  | 令牌 |      |

**json回复：**

根对象：

| 字段 | 类型 |   内容   |  备注   |
| :--: | :--: | :------: | :-----: |
| code | num  | 返回信息 | 1：成功 |
| msg  | str  | 提示信息 |         |
| res  | obj  | 信息本体 |         |

`res`对象：

|         字段          | 类型  |   内容   | 备注 |
| :-------------------: | :---: | :------: | :--: |
| deptBreifIntroduction |  str  | 部门简介 |      |
|        leader         |  str  | 书记姓名 |      |
|         phone         |  str  | 联系电话 |      |
|        tg_num         |  num  | 团干数量 |      |
|        tw_name        |  str  | 支部名称 |      |
|        ty_list        | obj[] | 信息本体 |      |

`ty_list`数组对象：

|    字段     | 类型 |   内容   |   备注   |
| :---------: | :--: | :------: | :------: |
|   avatar    | str  | 头像地址 | 相对路径 |
|    name     | str  | 成员姓名 |          |
| phonenumber | str  | 成员电话 |          |
|  s_number   | str  | 成员学号 |          |

### 个人成绩单

>  https://erke.smxpt.cn/lencon/app/erke/chengjidan 

请求方式：POST

认证方式：Token

**正文参数( application/x-www-form-urlencoded;charset=UTF-8 ):**

| 字段  | 类型 | 内容 | 备注 |
| :---: | :--: | :--: | :--: |
| token | str  | 令牌 |      |

**`json`回复：**

根对象：

| 字段 | 类型 |   内容   |  备注   |
| :--: | :--: | :------: | :-----: |
| code | num  | 返回信息 | 1：成功 |
| msg  | str  | 提示信息 |         |
| res  | obj  | 信息本体 |         |

`res`对象：

|    字段    |  类型  |     内容     |  备注   |
| :--------: | :----: | :----------: | :-----: |
| dept_name  |  str   |     班级     |         |
|  endTime   |  str   | 成绩生成时间 |         |
| indicator  | obj[]  |   信息本体   |         |
| indicators | obj[]  |   信息本体   |         |
|    name    |  str   |     姓名     |         |
| ruxue_date |  str   |   入学日期   | YYYY-MM |
|  s_number  |  str   |     学号     |         |
|    sort    |  str   |   排名信息   |   N/N   |
| totalScore |  num   |   获得学分   |         |
| typeScores | list[] |   <待分析>   |         |
|    yxmc    |  str   |   学院名称   |         |
|    zymc    |  str   |   专业名称   |         |

`indicator`列表对象：

| 字段 | 类型 |        内容        | 备注 |
| :--: | :--: | :----------------: | :--: |
| max  | num  | 此活动最多获得学分 |      |
| name | str  |    活动类型名称    |      |

`indicators`列表对象：

| 字段  | 类型 |        内容        | 备注 |
| :---: | :--: | :----------------: | :--: |
|  max  | num  | 此活动最多获得学分 |      |
| name  | str  |    活动类型名称    |      |
| score | num  | 在此活动中获得学分 |      |

### 团费缴纳

> https://erke.smxpt.cn/lencon/app/person/dues_info 

请求方式：POST

认证方式：Token

**正文参数( application/x-www-form-urlencoded;charset=UTF-8 ):**

| 字段  | 类型 | 内容 | 备注 |
| :---: | :--: | :--: | :--: |
| token | str  | 令牌 |      |

**json回复：**

根对象：

| 字段 | 类型 |   内容   |   备注   |
| :--: | :--: | :------: | :------: |
| code | num  | 返回信息 | 0：成功  |
| msg  | str  | 提示信息 |          |
| res  | obj  | 信息本体 | 暂未开放 |

### 我的社团

>  https://erke.smxpt.cn/lencon/app/person/get_asso_list 

请求方式：POST

认证方式：Token

**正文参数( application/x-www-form-urlencoded;charset=UTF-8 ):**

| 字段  | 类型 |    内容    | 备注 |
| :---: | :--: | :--------: | :--: |
| token | str  |    令牌    |      |
| page  | str  | 获取指定页 |      |

**json返回：**

根对象：

| 字段 | 类型 |   内容   |   备注   |
| :--: | :--: | :------: | :------: |
| code | num  | 返回信息 | 0：成功  |
| msg  | str  | 提示信息 |          |
| res  | obj  | 信息本体 | 暂未开放 |

### 认证情况

> https://erke.smxpt.cn/lencon/app/person/renzheng_info 

请求方式：POST

认证方式：Token

**正文参数( application/x-www-form-urlencoded;charset=UTF-8 ):**

| 字段  | 类型 |    内容    | 备注 |
| :---: | :--: | :--------: | :--: |
| token | str  |    令牌    |      |

**json返回：**

根对象：

|     字段      | 类型 |      内容      |          备注           |
| :-----------: | :--: | :------------: | :---------------------: |
|     code      | num  |    返回信息    |         0：成功         |
|      msg      | str  |    提示信息    |                         |
| phoneContent  | str  | 手机号认证情况 |                         |
| phoneRenzheng | num  | 手机号认证情况 | 1：已认证<br>0：未认证  |
|   wxContent   | str  |  微信认证情况  |                         |
|  wxRenzheng   | num  |  微信认证情况  | 1：已认证<br/>0：未认证 |

### 消息列表

> https://erke.smxpt.cn/lencon/app/index/message_list 

请求方式：POST

认证方式：Token

**正文参数( application/x-www-form-urlencoded;charset=UTF-8 ):**

| 字段  | 类型 |    内容    | 备注 |
| :---: | :--: | :--------: | :--: |
| token | str  |    令牌    |      |
| page  | str  | 获取指定页 |      |

**json返回：**

根对象：

| 字段 | 类型  |   内容   |  备注   |
| :--: | :---: | :------: | :-----: |
| code |  num  | 返回消息 | 0：成功 |
| msg  |  str  | 提示消息 |         |
| res  | obj[] | 信息本体 |         |
|      |       |          |         |

