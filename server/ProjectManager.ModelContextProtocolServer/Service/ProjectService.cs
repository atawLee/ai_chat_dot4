using ProjectManager.ModelContextProtocolServer.Models;

namespace ProjectManager.ModelContextProtocolServer.Service;

public class ProjectService : IProjectService
{
    private static readonly Dictionary<string, Project> Projects = new();

    public ProjectService()
    {
        InitializeSampleProjects();
    }

    private void InitializeSampleProjects()
    {
        var sampleProjects = new List<Project>
        {
            new Project
            {
                Id = Guid.NewGuid().ToString(),
                Name = "웹 애플리케이션 개발 프로젝트",
                Description = "React와 ASP.NET Core를 사용한 풀스택 웹 애플리케이션 개발",
                CreatedAt = DateTime.Now.AddDays(-30),
                UpdatedAt = DateTime.Now.AddDays(-5),
                TodoItems = new List<TodoItem>
                {
                    new TodoItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "프론트엔드 UI 설계",
                        Description = "Figma를 사용한 사용자 인터페이스 설계 및 프로토타입 제작",
                        IsCompleted = true,
                        CreatedAt = DateTime.Now.AddDays(-28),
                        CompletedAt = DateTime.Now.AddDays(-25)
                    },
                    new TodoItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "백엔드 API 개발",
                        Description = "RESTful API 설계 및 구현, 데이터베이스 설계",
                        IsCompleted = true,
                        CreatedAt = DateTime.Now.AddDays(-20),
                        CompletedAt = DateTime.Now.AddDays(-15)
                    },
                    new TodoItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "사용자 인증 시스템 구현",
                        Description = "JWT 토큰 기반 인증 시스템 개발",
                        IsCompleted = false,
                        CreatedAt = DateTime.Now.AddDays(-10)
                    },
                    new TodoItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "배포 환경 설정",
                        Description = "Docker를 사용한 컨테이너화 및 AWS 배포 설정",
                        IsCompleted = false,
                        CreatedAt = DateTime.Now.AddDays(-5)
                    }
                }
            },
            new Project
            {
                Id = Guid.NewGuid().ToString(),
                Name = "모바일 앱 개발 프로젝트",
                Description = "Flutter를 사용한 크로스플랫폼 모바일 애플리케이션 개발",
                CreatedAt = DateTime.Now.AddDays(-45),
                UpdatedAt = DateTime.Now.AddDays(-2),
                TodoItems = new List<TodoItem>
                {
                    new TodoItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "앱 기본 구조 설계",
                        Description = "Flutter 프로젝트 초기 설정 및 폴더 구조 설계",
                        IsCompleted = true,
                        CreatedAt = DateTime.Now.AddDays(-40),
                        CompletedAt = DateTime.Now.AddDays(-35)
                    },
                    new TodoItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "사용자 인터페이스 구현",
                        Description = "Material Design을 적용한 화면 구성 요소 개발",
                        IsCompleted = true,
                        CreatedAt = DateTime.Now.AddDays(-30),
                        CompletedAt = DateTime.Now.AddDays(-20)
                    },
                    new TodoItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "데이터 연동 기능 개발",
                        Description = "Firebase를 사용한 실시간 데이터베이스 연동",
                        IsCompleted = false,
                        CreatedAt = DateTime.Now.AddDays(-15)
                    },
                    new TodoItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "푸시 알림 기능 추가",
                        Description = "FCM을 사용한 푸시 알림 시스템 구현",
                        IsCompleted = false,
                        CreatedAt = DateTime.Now.AddDays(-8)
                    },
                    new TodoItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "앱스토어 배포 준비",
                        Description = "iOS App Store 및 Google Play Store 배포 준비",
                        IsCompleted = false,
                        CreatedAt = DateTime.Now.AddDays(-3)
                    }
                }
            },
            new Project
            {
                Id = Guid.NewGuid().ToString(),
                Name = "AI 챗봇 개발 프로젝트",
                Description = "Semantic Kernel과 Azure OpenAI를 활용한 지능형 챗봇 시스템 개발",
                CreatedAt = DateTime.Now.AddDays(-20),
                UpdatedAt = DateTime.Now.AddDays(-1),
                TodoItems = new List<TodoItem>
                {
                    new TodoItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "Azure OpenAI 서비스 설정",
                        Description = "Azure 계정 설정 및 OpenAI 리소스 생성",
                        IsCompleted = true,
                        CreatedAt = DateTime.Now.AddDays(-18),
                        CompletedAt = DateTime.Now.AddDays(-15)
                    },
                    new TodoItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "Semantic Kernel 통합",
                        Description = "Semantic Kernel 라이브러리 설정 및 기본 구조 구현",
                        IsCompleted = true,
                        CreatedAt = DateTime.Now.AddDays(-12),
                        CompletedAt = DateTime.Now.AddDays(-8)
                    },
                    new TodoItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "플러그인 시스템 개발",
                        Description = "OpenAPI 기반 외부 서비스 연동 플러그인 개발",
                        IsCompleted = false,
                        CreatedAt = DateTime.Now.AddDays(-7)
                    },
                    new TodoItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "채팅 인터페이스 구현",
                        Description = "SignalR을 사용한 실시간 채팅 인터페이스 구현",
                        IsCompleted = false,
                        CreatedAt = DateTime.Now.AddDays(-4)
                    }
                }
            },
            new Project
            {
                Id = Guid.NewGuid().ToString(),
                Name = "데이터 분석 플랫폼 구축",
                Description = "Python과 Machine Learning을 활용한 데이터 분석 및 시각화 플랫폼 개발",
                CreatedAt = DateTime.Now.AddDays(-60),
                UpdatedAt = DateTime.Now.AddDays(-10),
                TodoItems = new List<TodoItem>
                {
                    new TodoItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "데이터 수집 시스템 구축",
                        Description = "웹 크롤링 및 API를 통한 데이터 수집 파이프라인 구축",
                        IsCompleted = true,
                        CreatedAt = DateTime.Now.AddDays(-55),
                        CompletedAt = DateTime.Now.AddDays(-50)
                    },
                    new TodoItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "데이터 전처리 모듈 개발",
                        Description = "Pandas를 사용한 데이터 정제 및 변환 모듈 개발",
                        IsCompleted = true,
                        CreatedAt = DateTime.Now.AddDays(-45),
                        CompletedAt = DateTime.Now.AddDays(-35)
                    },
                    new TodoItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "머신러닝 모델 개발",
                        Description = "scikit-learn을 사용한 예측 모델 개발 및 훈련",
                        IsCompleted = false,
                        CreatedAt = DateTime.Now.AddDays(-25)
                    },
                    new TodoItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "대시보드 구현",
                        Description = "Streamlit을 사용한 인터랙티브 데이터 시각화 대시보드 구현",
                        IsCompleted = false,
                        CreatedAt = DateTime.Now.AddDays(-15)
                    }
                }
            },
            new Project
            {
                Id = Guid.NewGuid().ToString(),
                Name = "블록체인 DApp 개발",
                Description = "Ethereum 기반 탈중앙화 애플리케이션(DApp) 개발",
                CreatedAt = DateTime.Now.AddDays(-25),
                UpdatedAt = DateTime.Now.AddDays(-3),
                TodoItems = new List<TodoItem>
                {
                    new TodoItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "스마트 컨트랙트 설계",
                        Description = "Solidity를 사용한 스마트 컨트랙트 설계 및 작성",
                        IsCompleted = true,
                        CreatedAt = DateTime.Now.AddDays(-22),
                        CompletedAt = DateTime.Now.AddDays(-18)
                    },
                    new TodoItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "프론트엔드 Web3 연동",
                        Description = "React와 Web3.js를 사용한 프론트엔드 개발",
                        IsCompleted = false,
                        CreatedAt = DateTime.Now.AddDays(-14)
                    },
                    new TodoItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "테스트넷 배포",
                        Description = "Goerli 테스트넷에 스마트 컨트랙트 배포 및 테스트",
                        IsCompleted = false,
                        CreatedAt = DateTime.Now.AddDays(-7)
                    }
                }
            },
            new Project
            {
                Id = Guid.NewGuid().ToString(),
                Name = "IoT 센서 모니터링 시스템",
                Description = "Arduino와 Raspberry Pi를 활용한 IoT 센서 데이터 모니터링 시스템",
                CreatedAt = DateTime.Now.AddDays(-40),
                UpdatedAt = DateTime.Now.AddDays(-6),
                TodoItems = new List<TodoItem>
                {
                    new TodoItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "하드웨어 센서 설정",
                        Description = "온습도, 조도, 움직임 센서 연결 및 테스트",
                        IsCompleted = true,
                        CreatedAt = DateTime.Now.AddDays(-35),
                        CompletedAt = DateTime.Now.AddDays(-30)
                    },
                    new TodoItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "데이터 수집 펌웨어 개발",
                        Description = "Arduino IDE를 사용한 센서 데이터 수집 펌웨어 개발",
                        IsCompleted = true,
                        CreatedAt = DateTime.Now.AddDays(-25),
                        CompletedAt = DateTime.Now.AddDays(-20)
                    },
                    new TodoItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "클라우드 서버 연동",
                        Description = "AWS IoT Core를 통한 클라우드 서버 데이터 전송",
                        IsCompleted = false,
                        CreatedAt = DateTime.Now.AddDays(-15)
                    },
                    new TodoItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "실시간 모니터링 웹앱 구현",
                        Description = "센서 데이터의 실시간 시각화를 위한 웹 애플리케이션 개발",
                        IsCompleted = false,
                        CreatedAt = DateTime.Now.AddDays(-8)
                    }
                }
            }
        };

        foreach (var project in sampleProjects)
        {
            Projects[project.Id] = project;
        }

        Console.WriteLine($"샘플 프로젝트 {sampleProjects.Count}개가 초기화되었습니다.");
    }

    public ProjectCreateResult CreateProject(string name, string description)
    {
        var project = new Project
        {
            Id = Guid.NewGuid().ToString(),
            Name = name,
            Description = description,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        Projects[project.Id] = project;

        return new ProjectCreateResult
        {
            Success = true,
            Message = $"프로젝트 '{name}'이 생성되었습니다.",
            Project = project
        };
    }

    public ProjectListResult GetAllProjects()
    {
        return new ProjectListResult
        {
            Success = true,
            Message = Projects.Any() ? $"{Projects.Count}개의 프로젝트를 찾았습니다." : "등록된 프로젝트가 없습니다.",
            Projects = Projects.Values.ToList()
        };
    }

    public ProjectResult GetProject(string projectId)
    {
        if (!Projects.TryGetValue(projectId, out var project))
        {
            return new ProjectResult
            {
                Success = false,
                Message = $"ID '{projectId}'인 프로젝트를 찾을 수 없습니다.",
                Project = null
            };
        }

        return new ProjectResult
        {
            Success = true,
            Message = $"프로젝트 '{project.Name}' 정보를 조회했습니다.",
            Project = project
        };
    }

    public ProjectResult UpdateProject(string projectId, string? name = null, string? description = null)
    {
        if (!Projects.TryGetValue(projectId, out var project))
        {
            return new ProjectResult
            {
                Success = false,
                Message = $"ID '{projectId}'인 프로젝트를 찾을 수 없습니다.",
                Project = null
            };
        }

        if (!string.IsNullOrEmpty(name))
            project.Name = name;

        if (!string.IsNullOrEmpty(description))
            project.Description = description;

        project.UpdatedAt = DateTime.Now;

        return new ProjectResult
        {
            Success = true,
            Message = $"프로젝트가 수정되었습니다.",
            Project = project
        };
    }

    public ProjectResult DeleteProject(string projectId)
    {
        if (!Projects.TryGetValue(projectId, out var project))
        {
            return new ProjectResult
            {
                Success = false,
                Message = $"ID '{projectId}'인 프로젝트를 찾을 수 없습니다.",
                Project = null
            };
        }

        Projects.Remove(projectId);

        return new ProjectResult
        {
            Success = true,
            Message = $"프로젝트 '{project.Name}'이 삭제되었습니다.",
            Project = project
        };
    }

    public TodoResult AddTodoItem(string projectId, string title, string description = "")
    {
        if (!Projects.TryGetValue(projectId, out var project))
        {
            return new TodoResult
            {
                Success = false,
                Message = $"ID '{projectId}'인 프로젝트를 찾을 수 없습니다.",
                TodoItem = null
            };
        }

        var todoItem = new TodoItem
        {
            Id = Guid.NewGuid().ToString(),
            Title = title,
            Description = description,
            CreatedAt = DateTime.Now
        };

        project.TodoItems.Add(todoItem);
        project.UpdatedAt = DateTime.Now;

        return new TodoResult
        {
            Success = true,
            Message = $"할일 '{title}'이 추가되었습니다.",
            TodoItem = todoItem
        };
    }

    public TodoResult CompleteTodoItem(string projectId, string todoId)
    {
        if (!Projects.TryGetValue(projectId, out var project))
        {
            return new TodoResult
            {
                Success = false,
                Message = $"ID '{projectId}'인 프로젝트를 찾을 수 없습니다.",
                TodoItem = null
            };
        }

        var todoItem = project.TodoItems.FirstOrDefault(t => t.Id == todoId);
        if (todoItem == null)
        {
            return new TodoResult
            {
                Success = false,
                Message = $"ID '{todoId}'인 할일을 찾을 수 없습니다.",
                TodoItem = null
            };
        }

        todoItem.IsCompleted = true;
        todoItem.CompletedAt = DateTime.Now;
        project.UpdatedAt = DateTime.Now;

        return new TodoResult
        {
            Success = true,
            Message = $"할일 '{todoItem.Title}'이 완료되었습니다.",
            TodoItem = todoItem
        };
    }

    public TodoResult DeleteTodoItem(string projectId, string todoId)
    {
        if (!Projects.TryGetValue(projectId, out var project))
        {
            return new TodoResult
            {
                Success = false,
                Message = $"ID '{projectId}'인 프로젝트를 찾을 수 없습니다.",
                TodoItem = null
            };
        }

        var todoItem = project.TodoItems.FirstOrDefault(t => t.Id == todoId);
        if (todoItem == null)
        {
            return new TodoResult
            {
                Success = false,
                Message = $"ID '{todoId}'인 할일을 찾을 수 없습니다.",
                TodoItem = null
            };
        }

        project.TodoItems.Remove(todoItem);
        project.UpdatedAt = DateTime.Now;

        return new TodoResult
        {
            Success = true,
            Message = $"할일 '{todoItem.Title}'이 삭제되었습니다.",
            TodoItem = todoItem
        };
    }

    public ProjectStatsResult GetProjectStats()
    {
        var totalProjects = Projects.Count;
        var totalTodos = Projects.Values.Sum(p => p.TodoItems.Count);
        var completedTodos = Projects.Values.Sum(p => p.TodoItems.Count(t => t.IsCompleted));
        var completionRate = totalTodos > 0 ? (double)completedTodos / totalTodos * 100 : 0;

        return new ProjectStatsResult
        {
            Success = true,
            Message = totalProjects > 0 ? "프로젝트 통계를 조회했습니다." : "등록된 프로젝트가 없습니다.",
            Stats = new ProjectStats
            {
                TotalProjects = totalProjects,
                TotalTodos = totalTodos,
                CompletedTodos = completedTodos,
                CompletionRate = Math.Round(completionRate, 1)
            }
        };
    }
}
