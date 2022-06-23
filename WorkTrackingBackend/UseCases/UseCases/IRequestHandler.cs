namespace UseCases.UseCases;

public interface IRequestHandler<in T, out R>
{
    public R Handle(T request);
}