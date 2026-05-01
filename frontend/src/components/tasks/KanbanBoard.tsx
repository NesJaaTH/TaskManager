import { useState } from 'react'
import { Plus } from 'lucide-react'
import TaskCard from './TaskCard'
import TaskDetailSheet from './TaskDetailSheet'

interface Task {
  id: string
  title: string
  description: string
  status: string
  priority: string
  dueDate: string | null
  assigneeId: string | null
  createdById: string
}

const columns: { key: string; label: string; color: string; dot: string }[] = [
  { key: 'ToDo',       label: 'To Do',       color: 'text-white/50',  dot: 'bg-white/30' },
  { key: 'InProgress', label: 'In Progress', color: 'text-blue-400',  dot: 'bg-blue-400' },
  { key: 'Done',       label: 'Done',        color: 'text-emerald-400', dot: 'bg-emerald-400' },
  { key: 'Canceled',   label: 'Cancelled',   color: 'text-red-400/60', dot: 'bg-red-400/60' },
]

interface Props {
  tasks: Task[]
  projectId: string
  onRefresh: () => void
}

export default function KanbanBoard({ tasks, projectId, onRefresh }: Props) {
  const [selectedTask, setSelectedTask] = useState<Task | null>(null)

  const tasksByStatus = (status: string) =>
    tasks.filter(t => t.status === status)

  return (
    <>
      <div className="flex gap-4 overflow-x-auto pb-4 px-8">
        {columns.map(col => {
          const colTasks = tasksByStatus(col.key)
          return (
            <div key={col.key} className="shrink-0 w-72 flex flex-col">
              {/* Column header */}
              <div className="flex items-center gap-2 mb-3 px-1">
                <div className={`w-1.5 h-1.5 rounded-full ${col.dot}`} />
                <span className={`text-sm font-semibold font-heading ${col.color}`}>{col.label}</span>
                <span className="ml-auto text-xs text-white/25 font-medium bg-white/[0.05] rounded-full px-2 py-0.5">
                  {colTasks.length}
                </span>
              </div>

              {/* Tasks */}
              <div className="flex flex-col gap-2 flex-1 min-h-[120px]">
                {colTasks.map(task => (
                  <TaskCard key={task.id} task={task} onClick={() => setSelectedTask(task)} />
                ))}
              </div>

              {/* Add task */}
              <button className="flex items-center gap-2 mt-2 px-3 py-2 rounded-xl text-xs text-white/25 hover:text-white/50 hover:bg-white/[0.04] transition-all duration-150 w-full">
                <Plus className="w-3.5 h-3.5" />
                Add task
              </button>
            </div>
          )
        })}
      </div>

      <TaskDetailSheet
        task={selectedTask}
        projectId={projectId}
        onClose={() => setSelectedTask(null)}
        onRefresh={onRefresh}
      />
    </>
  )
}
