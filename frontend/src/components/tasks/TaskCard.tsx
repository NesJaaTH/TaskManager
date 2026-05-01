import { Calendar, User } from 'lucide-react'
import { formatDate, getInitials } from '@/lib/utils'

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

const priorityConfig: Record<string, { label: string; color: string }> = {
  High:   { label: 'High',   color: 'text-amber-400 bg-amber-400/10 ring-amber-400/20' },
  Medium: { label: 'Medium', color: 'text-blue-400 bg-blue-400/10 ring-blue-400/20' },
  Low:    { label: 'Low',    color: 'text-white/40 bg-white/[0.05] ring-white/10' },
}

interface Props {
  task: Task
  onClick: () => void
}

export default function TaskCard({ task, onClick }: Props) {
  const priority = priorityConfig[task.priority] ?? priorityConfig.Low

  return (
    <div
      onClick={onClick}
      className="glass-subtle rounded-xl p-3.5 cursor-pointer hover:border-[#A855F7]/20 hover:bg-[#A855F7]/[0.03] transition-all duration-150 group"
    >
      <div className="flex items-start justify-between gap-2 mb-2">
        <p className="text-sm font-medium text-white/90 line-clamp-2 group-hover:text-white transition-colors leading-snug">
          {task.title}
        </p>
        <span className={`shrink-0 text-[10px] font-semibold px-1.5 py-0.5 rounded-full ring-1 ${priority.color}`}>
          {priority.label}
        </span>
      </div>

      {task.description && (
        <p className="text-xs text-white/30 line-clamp-1 mb-2">{task.description}</p>
      )}

      <div className="flex items-center gap-2 mt-2">
        {task.dueDate && (
          <span className="flex items-center gap-1 text-[11px] text-white/30">
            <Calendar className="w-3 h-3" />
            {formatDate(task.dueDate)}
          </span>
        )}
        {task.assigneeId && (
          <div className="ml-auto w-5 h-5 rounded-full bg-[#A855F7]/20 border border-[#A855F7]/30 flex items-center justify-center">
            <User className="w-2.5 h-2.5 text-[#A855F7]" />
          </div>
        )}
      </div>
    </div>
  )
}
