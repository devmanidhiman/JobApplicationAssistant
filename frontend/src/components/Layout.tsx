import { NavLink, Outlet } from 'react-router-dom'

export default function Layout() {
  return (
    <div className="min-h-screen bg-background text-foreground">
      <nav className="border-b px-6 py-4 flex items-center gap-6">
        <span className="font-semibold text-lg mr-4">Job Application Assistant</span>
        <NavLink
          to="/submit"
          className={({ isActive }) =>
            isActive ? "text-primary font-medium" : "text-muted-foreground hover:text-foreground"
          }
        >
          Submit
        </NavLink>
        <NavLink
          to="/history"
          className={({ isActive }) =>
            isActive ? "text-primary font-medium" : "text-muted-foreground hover:text-foreground"
          }
        >
          History
        </NavLink>
      </nav>
      <main className="max-w-4xl mx-auto px-6 py-8">
        <Outlet />
      </main>
    </div>
  )
}