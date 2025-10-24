export function useLoading() {
  const _loading = useState<boolean>("loading", () => false);

  function setLoading() {
    _loading.value = true;
  }

  function setIdle() {
    _loading.value = false;
  }

  function isLoading() {
    return _loading.value;
  }

  return {
    isLoading,
    setLoading,
    setIdle,
  };
}
