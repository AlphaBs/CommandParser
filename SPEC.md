## CommandParser

CommandParser 는 입력 문자열을 Argument 목록으로 파싱하는 프로그램이다. 

### Word (단어)

문자열을 공백 문자를 기준으로 나눈 뒤 각 문자열을 word 라고 부른다.
큰따옴표로 둘러싸인 공백 문자는 word 를 나누는 데 사용되지 않는다.
공백 문자는 char.IsWhiteSpace 으로 결정하여 /t /n /r 같은 문자도 모두 공백 문자로 취급한다.
word 는 빈 문자열을 가질 수 없다.

예시
```
a b c         -> ["a", "b", "c"]
a "b c"       -> ["a", "b c"]
a \t b \n\n c -> ["a", "b", "c"]
    a  b      -> ["a", "b"]
```

### Key

마이너스 기호(-) 으로 시작하는 단어는 key 이다. 
큰따옴표 안에 포함된 마이너스 기호(-) 는 key 를 구분하는데 사용되지 않는다.

key 예시:
```
-key
--this-is-a-key
-
---
```

key 가 아닌 단어:
```
"-"
"-key"
```

### key=value

key 조건을 만족하면서 단어 안에 등호(=) 기호가 있다면 이 단어는 key=value 이다.
큰따옴표 안에 포함된 등호(=)는 key=value 를 구분하는데 사용되지 않는다.
첫 번째로 나오는 등호를 기준으로 단어를 앞/뒤로 나누어 앞부분을 key, 뒷부분을 value라고 한다.

key-value 예시:
```
-key=value     -> ("-key", "value")
--key=value    -> ("--key", "value")
--key="value"  -> ("--key", "\"value\"")
-key=          -> ("-key", "")
-key==a=       -> ("-key", "=a=")
-=             -> ("-", "")
```
key-value 가 아닌 단어:
```
key=value
-key"="value
--"key=value"
```

### Value

key 조건을 만족하지 않는 단어는 value 이다.

예시:
```
value
"v a l u e"
"="
"val=val"
```

### Argument

key 와 values 를 가지는 순서쌍이다.

key 는 null 을 허용하지 않는다.
values 는 value 의 목록이다. values 는 null, 빈 목록을 허용한다.

key-value 형태를 가지는 단어는 하나의 argument 으로 만들어진다.
key 는 그대로 가지고 values 에는 value 하나의 원소만 가지는 목록으로 순서쌍을 만든다.

key 이후에 value 가 연속될 경우, 연속되는 value 모두 values 가 되어 순서쌍을 만든다.
예시
```
--key1 value1 value2 value3-> [("--key1", ["value1", "value2", "value3"])]
```

## TODO

ArgumentBuilder
ArgumentFormat