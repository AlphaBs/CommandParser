## CommandParser

CommandParser 는 입력 문자열을 Argument 목록으로 파싱하는 프로그램이다. 주로 JVM 파라미터를 파싱하는데 사용된다.

### Argument

(key, value) 쌍이다. 여기서 key 와 value 는 모두 문자열 자료형이다.
key 는 공백 문자열이 될 수 있다. 
value 는 공백 문자열, null 이 될 수 있다.
key 가 공백 문자열이면서 value 가 null 인 argument 는 존재하지 않는다.

### Parsing

입력 문자열을 argument 리스트로 변환하는 과정이다.

1. 공백 문자를 기준으로 key 를 나눔
   공백 문자는 char.IsWhiteSpace 로 결정한다. \t \n \r 모두 포함
   예시: "a b c" -> ["a", "b", "c"]
2. key 안에 등호가 있는 경우, 첫 번째 등호를 기준으로 앞쪽이 key, 뒤쪽이 value 가 되어 한 쌍이 된다. 
   등호가 key 에 하나만 있으면서 마지막 문자일 경우, value 는 빈 문자열을 가진다. 등호가 없다면 value 는 null 이다.
   예시: "key=value" -> [("key", "value")]
   예시: "key=" -> [("key", "")] // key 에서 유일한 등호가 마지막에 위치하므로 value 는 빈 문자를 가진다.
   예시: "key" -> [("key", null)] // 등호가 없으면 value 는 null
3. 큰따옴표 사이에 있는 공백 문자와 등호는 위 규칙을 무시하고 문자 그대로 해석한다.
   예시: "key=\"a b c\"" -> [("key", "\"a b c\"")]
   예시: "\"key=value\"" -> [("\"key=value\"", null)]
   예시: "k\"e\"y=val\"ue\"" -> [("k\"e\"y", val\"ue\"")]

## TODO
