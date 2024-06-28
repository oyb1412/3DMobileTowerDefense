### 

# A New Kingdom


---

## Description

---


- 🔊프로젝트 소개

  A New Kingdom은 PC & Android 멀티 플랫폼을 지원하는 3D 타워디펜스 게임입니다. 타워를 설치하고 강화하는 클래식 방식을 채택했으며, 처음 경험하는 모바일이라는 환경에 최적화된 플레이 환경을 구축하는 것에 중점을 두었습니다.

- 개발 기간 : 2024.04.03 - 2024.04.13

- 🛠️사용 기술 및 개발 환경

   -언어 : C#
  
   -엔진 : Unity Engine
  
   -데이터베이스 : 로컬
  
   -개발 환경 : Window 10, Unity 2021.3.10f1



- 💻구동 화면

![스크린샷(22)](https://github.com/oyb1412/3DMobileTowerDefense/assets/154235801/b4f3b4c1-9408-4379-ab0c-7b0f6357e2df)

## 목차

---

- 기획 의도
- 발생 및 해결한 주요 문제점
- 핵심 로직


### 기획 의도

---

- PC 및 안드로이드 환경에서 안정적으로 구동되는 모바일 게임 개발.

- 터치, 슬라이드 등 모바일 환경만의 조작법 구현

- 모바일 환경에서의 데이터 관리 및 최적화된 성능 제공
  

### 발생 및 해결한 주요 문제점

---

- (발생)프레임 드랍 문제

   -발사체의 과도한 충돌 로직 및 고성능 셰이더 사용, 과한 파티클의 수로으로 인해 프레임 드랍 발생

   ![그림4](https://github.com/oyb1412/3DMobileTowerDefense/assets/154235801/07c78aae-2160-4380-bfc4-af7ca5a1c24a)

- (해결)에셋 수정

   -에셋의 코드를 수정해 불필요한 계산 제거

   ![그림5](https://github.com/oyb1412/3DMobileTowerDefense/assets/154235801/d43076ee-ae03-4392-a967-b139874d8966)

   -파티클 수 조정 및 모바일에 특화된 셰이더로 변경

   ![그림6](https://github.com/oyb1412/3DMobileTowerDefense/assets/154235801/87a99259-6a4a-44c2-863a-9b1a9910c13f)


### 핵심 로직

---
![Line_1_(1)](https://github.com/oyb1412/TinyDefense/assets/154235801/f664c47e-d52b-4980-95ec-9859dea11aab)
### ・모바일 & PC 크로스 플랫폼 인풋 시스템

모바일과 PC 각각에 알맞은 인풋 시스템을 구현해 어떤 기기에서도 게임 플레이 가능

![그림1](https://github.com/oyb1412/3DMobileTowerDefense/assets/154235801/e0c9baa0-f54a-40a4-a898-af6dc034a6c0)
![Line_1_(1)](https://github.com/oyb1412/TinyDefense/assets/154235801/f664c47e-d52b-4980-95ec-9859dea11aab)

### ・JSON을 이용한 로컬 데이터 관리

게임 데이터를 외부에서 관리할 필요성을 느껴 JSON을 이용해 데이터 보안을 강화하고 유지보수의 용이성 극대화

![3](https://github.com/oyb1412/3DMobileTowerDefense/assets/154235801/5e9f205b-7372-4c1f-b86d-0693d265c0c6)
![Line_1_(1)](https://github.com/oyb1412/TinyDefense/assets/154235801/f664c47e-d52b-4980-95ec-9859dea11aab)
### ・게임 저장 기능
게임 도중 종료 시 데이터가 저장되지 않아 처음부터 다시 시작해야 하는 문제를 해결하기 위해 게임 저장 기능 추가

![그림2](https://github.com/oyb1412/3DMobileTowerDefense/assets/154235801/60d268cc-1699-4f50-92e9-42f848770a8c)
![Line_1_(1)](https://github.com/oyb1412/TinyDefense/assets/154235801/f664c47e-d52b-4980-95ec-9859dea11aab)

### ・공통된 ‘선택’ 이라는 기능의 추상화

클릭이나 터치로 선택할 수 있는 오브젝트(적, 타워, 땅 등)를 통합하여 코드의 양을 줄이고 유지보수의 용이성 극대화

![6](https://github.com/oyb1412/3DMobileTowerDefense/assets/154235801/f9f7e739-9092-40fc-87fa-f7a45dd040fa)
![Line_1_(1)](https://github.com/oyb1412/TinyDefense/assets/154235801/f664c47e-d52b-4980-95ec-9859dea11aab)

### ・옵저버 패턴을 이용한 UI 시스템

데이터 변경이 없을 때도 주기적으로 UI를 업데이트하는 문제를 해결하여 퍼포먼스를 최적화

![그림3](https://github.com/oyb1412/3DMobileTowerDefense/assets/154235801/290b892b-fcbe-405a-a30c-0f6b2b1afc0c)
![Line_1_(1)](https://github.com/oyb1412/TinyDefense/assets/154235801/f664c47e-d52b-4980-95ec-9859dea11aab)
### ・씬 전환 페이드 시스템

극적인 씬 전환 연출을 위해 페이드 전환 효과와 트위닝 사용.

![9](https://github.com/oyb1412/3DMobileTowerDefense/assets/154235801/f21184a1-1bf9-415b-847a-758df98ca54c)
![Line_1_(1)](https://github.com/oyb1412/TinyDefense/assets/154235801/f664c47e-d52b-4980-95ec-9859dea11aab)

### ・풀링 오브젝트 시스템

런타임 시 객체 생성과 제거를 방지하고, 성능을 높이기 위해 풀링 시스템 사용.

![10](https://github.com/oyb1412/3DMobileTowerDefense/assets/154235801/fb002569-4b6f-4162-800f-215b522a42f9)
